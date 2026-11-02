using System.Text.Json;
using System.Text.Json.Nodes;

namespace RBSoftSkkm;

// Входные свойства
public sealed partial class SkkmConnector
{
    /// <summary>
    /// Касса и кассир — основа запроса документа (смена, отчёты, денежный ящик). Кассир уходит всегда, даже пустой.
    /// </summary>
    private JsonObject CheckBase() => Obj(
        ("DeviceName", DeviceName),
        ("Cashier", Obj(("Name", CashierName.Trim()), ("Vatin", CashierVatin.Trim()))));

    /// <summary>
    /// Чек (POST check) и чек коррекции ФФД 1.2 (POST correction120). Данные коррекции — если заполнены.
    /// </summary>
    private JsonObject CheckBody() => Add(CheckBase(),
        ("PaymentType", (int)PaymentType),
        ("TaxVariant", (int)TaxVariant),
        ("CorrectionData", CorrectionJson()),
        ("Customer", CustomerJson()),
        ("SaleLocation", SaleLocation),
        ("SaleAddress", SaleAddress),
        ("SenderEmail", SenderEmail),
        ("OperationOnline", IsOperationOnline),
        ("IndustryAttribute", Group(
            ("IdentifierFoiv", IndustryAttributeIdentifierFoiv), ("DocumentDate", IndustryAttributeDocumentDate),
            ("DocumentNumber", IndustryAttributeDocumentNumber),
            ("AttributeValue", NullIfEmpty(IndustryAttributeAttributeValue)))),
        ("UserAttribute", Group(("Name", UserAttributeName), ("Value", UserAttributeValue))),
        ("OperationalAttribute", Group(
            ("DateTime", OperationalAttributeDateTime), ("OperationId", OperationalAttributeOperationID),
            ("OperationData", OperationalAttributeOperationData))),
        ("ElectronicPaymentInfo", ElectronicPaymentInfo()),
        ("Electronically", IsElectronically),
        ("TimeZone", (int?)TimeZone),
        ("TextBefore", TextBefore),
        ("TextAfter", TextAfter),
        // Пустой дополнительный реквизит (тег 1192) не передаётся.
        ("AdditionalAttribute", NullIfEmpty(AdditionalAttribute)),
        ("AgentSign", (int?)AgentSign),
        ("AgentData", Group(
            ("PayingAgentOperation", PayingAgentOperation),
            ("PayingAgentPhone", EmptyArrayAsNull(PayingAgentPhone)),
            ("ReceivePaymentsOperatorPhone", EmptyArrayAsNull(ReceivePaymentsOperatorPhone)),
            ("MoneyTransferOperatorPhone", EmptyArrayAsNull(MoneyTransferOperatorPhone)),
            ("MoneyTransferOperatorName", MoneyTransferOperatorName),
            ("MoneyTransferOperatorAddress", NullIfEmpty(MoneyTransferOperatorAddress)),
            ("MoneyTransferOperatorVatin", MoneyTransferOperatorVatin))),
        ("Vendor", Group(("Name", VendorDataName), ("Phones", VendorDataPhones), ("Vatin", VendorDataVatin))),
        ("Payments", PaymentsJson()),
        ("Positions", new JsonArray(Positions.SelectMany(PositionJson).ToArray())));

    /// <summary>
    /// Чек коррекции ФФД 1.05 (POST correction105): суммы по ставкам вместо позиций.
    /// </summary>
    private JsonObject Correction105Body() => Add(CheckBase(),
        ("DocId", ExplicitDocId),
        ("PaymentType", (int)PaymentType),
        ("TaxVariant", (int)TaxVariant),
        ("Customer", CustomerJson()),
        ("AdditionalAttribute", NullIfEmpty(AdditionalAttribute)),
        ("CorrectionData", CorrectionJson()),
        ("Payments", PaymentsJson()),
        // Не задана — 0. Это значение, а не пропущенный тег.
        ("SumTaxNone", SumTaxNone ?? 0m), ("SumTax0", SumTax0 ?? 0m), ("SumTax5", SumTax5 ?? 0m), ("SumTax7", SumTax7 ?? 0m),
        ("SumTax10", SumTax10 ?? 0m), ("SumTax18", SumTax18 ?? 0m), ("SumTax20", SumTax20 ?? 0m), ("SumTax22", SumTax22 ?? 0m),
        ("SumTax105", SumTax105 ?? 0m), ("SumTax107", SumTax107 ?? 0m), ("SumTax110", SumTax110 ?? 0m),
        ("SumTax118", SumTax118 ?? 0m), ("SumTax120", SumTax120 ?? 0m), ("SumTax122", SumTax122 ?? 0m));

    /// <summary>
    /// Слип (POST slip): сначала многострочный Text, затем нефискальные строки Positions. Кассир не передаётся.
    /// </summary>
    private JsonObject SlipBody()
    {
        var lines = Text.Length == 0 ? [] : Text.Replace("\r\n", "\n").Split('\n').Select(line => TextLine(line, null, null));
        return Obj(("DeviceName", DeviceName), ("Positions", new JsonArray(lines.Concat(Positions.Where(row => !row.FiscalString).Select(LineJson)).ToArray())));
    }

    /// <summary>
    /// Внесение / выемка (POST cashin, cashout).
    /// </summary>
    private JsonObject CashBody() => Add(CheckBase(), ("Sum", CashAmount));

    /// <summary>
    /// Покупатель (Customer) — в чеке и в чеке коррекции 1.05; null, если ни одно поле не заполнено.
    /// </summary>
    private JsonObject? CustomerJson() => Group(
        ("Info", CustomerInfo), ("Vatin", CustomerVatin), ("Email", CustomerEmail), ("Phone", CustomerPhone),
        ("DateOfBirth", CustomerDateOfBirth), ("Citizenship", CustomerCitizenship),
        ("DocumentTypeCode", CustomerDocumentTypeCode), ("DocumentData", CustomerDocumentData), ("Address", CustomerAddress));

    /// <summary>
    /// Данные коррекции; null, если ни одно поле не заполнено.
    /// </summary>
    private JsonObject? CorrectionJson() => Group(
        ("Type", (int?)CorrectionDataType),
        ("Description", CorrectionDataDescription),
        ("Date", CorrectionDataDate),
        ("Number", CorrectionDataNumber));

    /// <summary>
    /// Оплаты — всегда все пять сумм, в том числе нули.
    /// </summary>
    private JsonObject PaymentsJson() => Obj(
        ("Cash", Cash), ("ElectronicPayment", ElectronicPayment), ("AdvancePayment", AdvancePayment),
        ("Credit", Credit), ("CashProvision", CashProvision));

    /// <summary>
    /// Тело RequestKM. Необязательные поля уходят вместе с обязательными, в том числе false и 0.
    /// </summary>
    private JsonObject RequestKmBody() => Obj(
        ("DeviceName", DeviceName),
        ("RequestKM", Obj(
            ("Guid", Guid),
            ("NotSendToServer", NotSendToServer),
            ("WaitForResult", WaitForResult),
            ("MarkingCode", MarkingCode),
            ("PlannedStatus", (int)PlannedStatus),
            ("Quantity", Quantity),
            ("MeasureOfQuantity", (int)MeasureOfQuantity),
            ("FractionalQuantityNumerator", FractionalQuantityNumerator),
            ("FractionalQuantityDenominator", FractionalQuantityDenominator))));

    /// <summary>
    /// Сведения о безналичной оплате (ElectronicPaymentInfo) из AddElectronicPaymentInfo; null, если их нет.
    /// Обязательна только сумма. Способ оплаты, идентификаторы и доп. сведения без значения в запрос не входят:
    /// у способа оплаты 0 — это предоплата 100%, а не «не задано».
    /// </summary>
    private JsonArray? ElectronicPaymentInfo()
        => _electronicPayments.Count == 0 ? null : new JsonArray(_electronicPayments
            .Select(p => (JsonNode)Obj(
                ("Amount", p.Amount),
                ("PaymentMethod", p.Method is { } method ? (int)method : null),
                ("Identifiers", NullIfEmpty(p.Identifiers)),
                ("AdditionalInformation", NullIfEmpty(p.Information))))
            .ToArray());

    /// <summary>
    /// Строка чека; текст фискальной строки печатается отдельной строкой после неё.
    /// </summary>
    private static IEnumerable<JsonNode> PositionJson(Position row)
    {
        yield return LineJson(row);
        if (row.FiscalString && !string.IsNullOrEmpty(row.Text))
            yield return TextLine(row.Text, row.TextFont, row.TextAlignment);
    }

    /// <summary>
    /// Одна строка документа — по заполненным колонкам: товар, картинка, штрихкод, линия или текст.
    /// </summary>
    private static JsonObject LineJson(Position row)
    {
        if (row.FiscalString)
            return Obj(("FiscalString", FiscalString(row)));
        if (!string.IsNullOrEmpty(row.PictureValue))
            return Obj(("Picture", Obj(("Value", row.PictureValue), ("Alignment", (int)row.PictureAlignment))));
        if (!string.IsNullOrEmpty(row.BarcodeValue))
            return Barcode(row.BarcodeType, row.BarcodeValue, row.BarcodeAlignment);
        if (row.lineStyle.HasValue)
            return Obj(("SeparatorLine", Obj(("LineStyle", (int)row.lineStyle.Value))));
        return TextLine(row.Text ?? "", row.Font, row.Alignment);
    }

    /// <summary>
    /// Товар или услуга (FiscalString). Плоские колонки Position собираются во вложенные блоки сервера.
    /// </summary>
    private static JsonObject FiscalString(Position row) => Obj(
        ("Name", row.Name),
        ("Quantity", row.Quantity),
        ("PriceWithDiscount", row.PriceWithDiscount),
        ("SumWithDiscount", row.SumWithDiscount),
        ("DiscountSum", row.DiscountSum),
        ("Tax", row.Tax),
        ("TaxSum", row.TaxSum),
        ("Department", row.Department),
        ("SignMethodCalculation", (int?)row.SignMethodCalculation),
        ("SignCalculationObject", (int?)row.SignCalculationObject),
        // Не задана — штуки (0), тег в строке есть.
        ("MeasureOfQuantity", (int)(row.MeasureOfQuantity ?? MeasureOfQuantity.Piece)),
        // Без единицы измерения — «шт».
        ("MeasurementUnit", string.IsNullOrEmpty(row.MeasurementUnit) ? "шт" : row.MeasurementUnit),
        ("ExciseAmount", row.ExciseAmount),
        ("CountryOfOrigin", row.CountryOfOrigin),
        ("CustomsDeclaration", row.CustomsDeclaration),
        ("SignSubjectCalculationAgent", (int?)row.SignSubjectCalculationAgent),
        // Блоки агента, поставщика и отраслевого реквизита — только при заполненном главном поле.
        ("AgentData", string.IsNullOrEmpty(row.PayingAgentOperation) ? null : Obj(
            ("PayingAgentOperation", row.PayingAgentOperation),
            ("PayingAgentPhone", EmptyArrayAsNull(row.PayingAgentPhone)),
            ("ReceivePaymentsOperatorPhone", EmptyArrayAsNull(row.ReceivePaymentsOperatorPhone)),
            ("MoneyTransferOperatorPhone", EmptyArrayAsNull(row.MoneyTransferOperatorPhone)),
            ("MoneyTransferOperatorName", NullIfEmpty(row.MoneyTransferOperatorName)),
            ("MoneyTransferOperatorAddress", NullIfEmpty(row.MoneyTransferOperatorAddress)),
            ("MoneyTransferOperatorVatin", NullIfEmpty(row.MoneyTransferOperatorVATIN)))),
        ("Vendor", string.IsNullOrEmpty(row.VendorDataName) ? null : Obj(
            ("Name", row.VendorDataName), ("Phones", row.VendorDataPhones), ("Vatin", row.VendorDataVatin))),
        // Дробное количество сервер применяет, только когда заданы обе части.
        ("FractionalQuantity", row.FractionalQuantityNumerator > 0 && row.FractionalQuantityDenominator > 0
            ? Obj(("Numerator", row.FractionalQuantityNumerator), ("Denominator", row.FractionalQuantityDenominator)) : null),
        // Данные кода товара: Cis — в Barcode как есть, Cis_Base64 — в MarkingCode.
        ("GoodCodeData", Group(
            ("Gtin", NullIfEmpty(row.GTIN)), ("SerialNumber", NullIfEmpty(row.SerialNumber)),
            ("CommodityGroup", NullIfEmpty(row.CommodityGroup)), ("Barcode", NullIfEmpty(row.Cis)),
            ("MarkingCode", NullIfEmpty(row.Cis_Base64)), ("StampType", NullIfEmpty(row.StampType)))),
        ("IndustryAttribute", string.IsNullOrEmpty(row.IndustryAttributeIdentifierFoiv) ? null : Obj(
            ("IdentifierFoiv", row.IndustryAttributeIdentifierFoiv),
            ("DocumentDate", NullIfEmpty(row.IndustryAttributeDocumentDate)),
            ("DocumentNumber", NullIfEmpty(row.IndustryAttributeDocumentNumber)),
            ("AttributeValue", NullIfEmpty(row.IndustryAttributeAttributeValue)))),
        ("AdditionalAttribute", NullIfEmpty(row.AdditionalAttribute)));

    /// <summary>
    /// Штрихкод. Значение в Base64 уходит в ValueBase64, иначе в Value.
    /// </summary>
    private static JsonObject Barcode(BarcodeType type, string value, PrintAlignment? alignment)
        => Obj(("Barcode", Obj(
            ("Type", type.ToString()), (IsBase64(value) ? "ValueBase64" : "Value", value),
            ("Alignment", alignment?.ToString().ToLowerInvariant()))));

    /// <summary>
    /// Строка в Base64: есть латинская буква и строка декодируется.
    /// </summary>
    private static bool IsBase64(string value)
        => value.Any(ch => ch is >= 'A' and <= 'Z' or >= 'a' and <= 'z')
           && Convert.TryFromBase64String(value, new byte[value.Length], out _);

    /// <summary>
    /// Строка текста с разметкой в начале: [center], [big], [QR], [line], [line,dotted], [dotted] —
    /// уходит как текст, штрихкод или разделительная линия. Текст вида "[Промо]" без известных тегов печатается как есть.
    /// </summary>
    private static JsonObject TextLine(string line, PrintFont? font, PrintAlignment? alignment)
    {
        BarcodeType? barcodeType = null;
        LineStyle? lineStyle = null;
        var hasLineTag = false;

        var close = line.StartsWith("[") ? line.IndexOf(']') : -1;
        if (close > 0)
        {
            var tags = line[1..close].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            hasLineTag = tags.Any(tag => tag.Equals("line", StringComparison.OrdinalIgnoreCase));
            var recognized = hasLineTag;
            foreach (var tag in tags.Where(tag => !char.IsDigit(tag[0])))
            {
                // [bold] — шрифт; линия жирная, только вместе с [line].
                if (Enum.TryParse<LineStyle>(tag, true, out var style) && (style != LineStyle.Bold || hasLineTag))
                    lineStyle = style;
                else if (Enum.TryParse<BarcodeType>(tag, true, out var barcode))
                    barcodeType = barcode;
                else if (Enum.TryParse<PrintAlignment>(tag, true, out var parsedAlignment))
                    alignment = parsedAlignment;
                else if (Enum.TryParse<PrintFont>(tag, true, out var parsedFont))
                    font = parsedFont;
                else
                    continue;
                recognized = true;
            }
            if (recognized)
                line = line[(close + 1)..];
        }

        if (hasLineTag || (lineStyle != null && line.Length == 0))
            return Obj(("SeparatorLine", Obj(("LineStyle", (int)(lineStyle ?? LineStyle.Solid)))));
        if (barcodeType != null)
            return Barcode(barcodeType.Value, line.Trim(), alignment);
        return Obj(("TextString", Obj(
            ("Text", line), ("Font", font?.ToString()), ("Alignment", alignment?.ToString().ToLowerInvariant()))));
    }

    private static string? NullIfEmpty(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>Пустой массив телефона не передаётся.</summary>
    private static string[]? EmptyArrayAsNull(string[]? values) => values is { Length: > 0 } ? values : null;

    /// <summary>
    /// JSON-объект из пар «имя — значение»; null пропускается.
    /// </summary>
    private static JsonObject Obj(params (string Name, object? Value)[] fields) => Add(new JsonObject(), fields);

    private static JsonObject Add(JsonObject target, params (string Name, object? Value)[] fields)
    {
        foreach (var (name, value) in fields)
            if (value != null)
                target[name] = value as JsonNode ?? JsonSerializer.SerializeToNode(value, value.GetType());
        return target;
    }

    /// <summary>
    /// Блок запроса. Нет ни одного заполненного поля — null, блок не уходит.
    /// Если блок есть, пустые строки внутри него остаются: это пустое значение, а не пропущенное поле.
    /// </summary>
    private static JsonObject? Group(params (string Name, object? Value)[] fields)
        => fields.All(f => f.Value is null or "" or 0m || f.Value is Array { Length: 0 }) ? null : Obj(fields);
}
