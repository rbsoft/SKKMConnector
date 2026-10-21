using System.Globalization;
using RBSoftSkkm.Internal;

namespace RBSoftSkkm;

// Раскладка ответов сервера по свойствам коннектора: по одному Apply-методу на тип ответа.
public sealed partial class SkkmConnector
{
    /// <summary>
    /// Раскладывает сведения об устройстве
    /// </summary>
    private void ApplyDeviceInfo(Device? device)
    {
        if (device == null)
            return;

        DeviceModel = device.Model ?? "";
        DeviceSerialNumber = device.SerialNumber ?? "";
        DeviceFirmwareVersion = device.FirmwareVersion ?? "";
        DeviceConfigurationVersion = device.ConfigurationVersion ?? "";
        DeviceFfdVersion = device.FfdVersion ?? "";
        FnFfdVersion = device.FnFfdVersion ?? "";
        DeviceClass = device.DeviceClass;
        DeviceTimeZone = device.TimeZone;
        IsFiscal = device.IsFiscal;

        // Ширину строки в GetKktInfo присылает и статус ККТ — нулём из устройства не затираем.
        if (device.LineLength > 0)
            LineLength = device.LineLength;
        if (device.LineLengthPixels > 0)
            LineLengthPixels = device.LineLengthPixels;
    }

    /// <summary>
    /// Раскладывает статус ККТ 
    /// </summary>
    private void ApplyStatus(KktStatus? status)
    {
        Status = status;
        if (status == null)
            return;

        ShiftNumber = status.ShiftNumber;
        CheckNumber = status.DocNumber;
        CurrentShiftState = status.ShiftState;
        ComputerTime = status.ComputerTime;
        DeviceTime = status.DeviceTime;
        LineLength = status.LineLength;
        LineLengthPixels = status.LineLengthPixels;
        IsFnPresent = status.IsFnPresent;
        IsFiscal = status.IsFiscal;
        FnWarnings = status.Warnings;
        _backlog = status.Backlog;
    }

    /// <summary>
    /// Раскладывает сведения о фискальном накопителе 
    /// </summary>
    private void ApplyFnInfo(Fn? fn)
    {
        if (fn == null)
            return;

        FnNumber = fn.SerialNumber ?? "";
        RnNumber = fn.RnNumber ?? "";
        FnsUrl = fn.FnsUrl ?? "";
        if (fn.ValidityDate != default)
            FnValidityDate = fn.ValidityDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Выходные параметры смены/отчёта: из результата операции (outputParameters) и из документа (OutputParameters).
    /// </summary>
    private void ApplyOutputParameters(FiscalOutputParameters? output)
    {
        if (output == null)
            return;

        if (output.ShiftState.HasValue)
            CurrentShiftState = output.ShiftState;
        if (output.NumberOfChecks > 0)
            CheckNumberInShift = output.NumberOfChecks;
        if (output.NumberOfDocuments > 0)
            ShiftDocumentsCount = output.NumberOfDocuments;
        if (output.ShiftClosingCheckNumber > 0)
            ShiftClosingCheckNumber = output.ShiftClosingCheckNumber;
        if (!string.IsNullOrEmpty(output.DateTime))
        {
            FiscalDateTime = output.DateTime!;
            DeviceDateTime = output.DateTime!;
        }
        if (output.ShiftNumber > 0)
            ShiftNumber = output.ShiftNumber;
        if (output.CheckNumber > 0)
            CheckNumber = output.CheckNumber;
        CashBalance = output.CashBalance;
        if (!string.IsNullOrEmpty(output.FnValidityDate))
            FnValidityDate = output.FnValidityDate!;
        if (output.ResourcesFn > 0)
            FnDaysResources = output.ResourcesFn;

        if (output.Backlog != null)
            _backlog = output.Backlog;

        if (output.FnWarnings != null)
            FnWarnings = output.FnWarnings;
    }

    /// <summary>
    /// Шаблон печати из ответа
    /// </summary>
    private void ApplyTemplate(PrintTemplate template)
    {
        PrintTemplate = template;
        if (!string.IsNullOrEmpty(template.Name))
            TemplateName = template.Name;
        TemplateType = template.Type;
        RestorePositionsFromPrintLines(template.Lines);
    }

    /// <summary>
    /// Шаблон чека из ответа
    /// </summary>
    private void ApplyCheckTemplate(CheckTemplate template)
    {
        CheckTemplate = template;
        CheckTemplateDocument = template.Document;
        if (!string.IsNullOrEmpty(template.Name))
            TemplateName = template.Name;
        if (template.Document != null)
            RestoreCheckFromTemplate(template.Document);
    }

    private void ApplyOperation(DeviceTaskInfo operation)
    {
        Operation = operation;
        DocumentId = operation.DocId ?? "";
    }

    private void ApplyDocument(CheckDocument document)
    {
        Check = document;
        ApplyDeviceInfo(document.DeviceInfo);
        ShiftTotals = document.ShiftTotal;
        OverallTotals = document.OverallTotals;
        PrintForm = document.Lines?.Select(x => new PrintLine(x)).ToList() ?? [];
        DocumentId = document.DocId ?? "";

        FiscalSign = document.FiscalSign ?? "";
        FnNumber = document.Fn ?? "";
        // РНМ и сайт ФНС документ передаёт только в шапке.
        RnNumber = document.DocumentHeader?.RnNumber ?? "";
        FnsUrl = document.DocumentHeader?.FnsUrl ?? "";

        // Номера, наличные и время уже могли прийти из выходных параметров отчёта — не затираем нулями.
        if (document.ShiftNumber > 0)
            ShiftNumber = document.ShiftNumber;
        if (document.DocNumber > 0)
            CheckNumber = document.DocNumber;
        if (document.DocNumberInShift > 0)
            CheckNumberInShift = document.DocNumberInShift;
        if (document.CashSum.HasValue)
            CashBalance = document.CashSum.Value;
        if (document.Date != default)
            ServerDateTime = document.Date.ToString("o", CultureInfo.InvariantCulture);
        if (document.FiscalDate != default)
            FiscalDateTime = DeviceDateTime = document.FiscalDate.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Общие поля результата операции (сервер: CheckbaseParams).
    /// </summary>
    private void ApplyCheckbaseResult(CheckbaseResult result)
    {
        DocumentId = result.DocId ?? "";
        ShiftNumber = result.ShiftNumber;
        FnsUrl = result.FnsUrl ?? "";
        FnNumber = result.FnNumber ?? "";
        RnNumber = result.RnNumber ?? "";
        ServerDateTime = result.DateTime ?? "";
        FiscalDateTime = DeviceDateTime = result.FiscalDateTime ?? "";
    }

    /// <summary>
    /// Результат фискального чека и чеков коррекции 
    /// </summary>
    private void ApplyFiscalParams(FiscalParamsResult result)
    {
        ApplyCheckbaseResult(result);
        FiscalSign = result.FiscalSign ?? "";
        CheckNumber = result.FiscalNumber;
        ApplyDeviceInfo(result.DeviceInfo);
    }

    /// <summary>
    /// Результат открытия смены (сервер: OpenShiftResult).
    /// </summary>
    private void ApplyOpenShiftResult(OpenShiftResult result)
    {
        ApplyCheckbaseResult(result);
        ApplyOutputParameters(result.OutputParameters);
        ApplyDeviceInfo(result.DeviceInfo);
    }

    /// <summary>
    /// Результат Z/X-отчёта и отчёта о расчётах (сервер: ReportResult).
    /// </summary>
    private void ApplyReportResult(ReportResult result)
    {
        ApplyCheckbaseResult(result);
        if (result.ShiftTotal != null)
            ShiftTotals = result.ShiftTotal;
        if (result.OverallTotals != null)
            OverallTotals = result.OverallTotals;
        ApplyOutputParameters(result.OutputParameters);
        ApplyDeviceInfo(result.DeviceInfo);
    }

    /// <summary>
    /// Документ из ответа (сервер: CheckBase и наследники — чек, отчёт, внесение, слип и т.д.).
    /// </summary>
    private void ApplyCheckBaseDoc(CheckDocument doc)
    {
        // Пустая дата сервера ("0001-01-01T00:00:00") остаётся как есть; непустую ApplyDocument заменит.
        ServerDateTime = doc.Date.ToString("s", CultureInfo.InvariantCulture);
        FiscalDateTime = DeviceDateTime = doc.FiscalDate.ToString("s", CultureInfo.InvariantCulture);
        ApplyOutputParameters(doc.OutputParameters);
        ApplyDocument(doc);
    }

    private void ApplyCheckList(CheckDocument[] docs)
        => Checks = docs.Select(d => new Check(d)).ToList();
}
