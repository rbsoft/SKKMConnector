using RBSoftSkkm.Internal;

namespace RBSoftSkkm;

public sealed partial class SkkmConnector
{
    /// <summary>
    /// Код ошибки для проверок на стороне клиента.
    /// </summary>
    private const int ValidationErrorCode = -1;

    /// <summary>
    /// Проверка обязательных полей чека перед отправкой
    /// </summary>
    private bool ValidateCheck()
    {
        if (string.IsNullOrWhiteSpace(DeviceName))
            return FailValidation("Не указано имя кассы.");

        return ValidatePositions();
    }

    /// <summary>
    /// Проверка перед отправкой чека коррекции ФФД 1.2: имя кассы, тип чека коррекции и товарная часть.
    /// </summary>
    private bool ValidateCorrection120()
    {
        if (string.IsNullOrWhiteSpace(DeviceName))
            return FailValidation("Не указано имя кассы.");
        if (!IsCorrectionType(PaymentType))
            return FailValidation($"Тип операции {PaymentType} не является чеком коррекции. Ожидается CorrectionSale, CorrectionSaleReturn, CorrectionPurchase или CorrectionPurchaseReturn.");

        return ValidatePositions();
    }

    /// <summary>
    /// Проверка перед отправкой чека коррекции ФФД 1.0.5: имя кассы и тип чека коррекции (позиции не передаются).
    /// </summary>
    private bool ValidateCorrection105()
    {
        if (string.IsNullOrWhiteSpace(DeviceName))
            return FailValidation("Не указано имя кассы.");
        if (!IsCorrectionType(PaymentType))
            return FailValidation($"Тип операции {PaymentType} не является чеком коррекции. Ожидается CorrectionSale, CorrectionSaleReturn, CorrectionPurchase или CorrectionPurchaseReturn.");

        return true;
    }

    /// <summary>
    /// Тип чека является чеком коррекции: приход, возврат прихода, расход или возврат расхода.
    /// </summary>
    private static bool IsCorrectionType(CheckType type)
        => type is CheckType.CorrectionSale
            or CheckType.CorrectionSaleReturn
            or CheckType.CorrectionPurchase
            or CheckType.CorrectionPurchaseReturn;

    /// <summary>
    /// Проверка товарной части: в документе должна быть хотя бы одна фискальная строка
    /// с наименованием, количеством и признаками расчёта.
    /// </summary>
    private bool ValidatePositions()
    {
        var fiscalCount = 0;
        var number = 0;
        foreach (var position in _positions)
        {
            number++;
            if (position is not FiscalLine line)
                continue;

            fiscalCount++;
            if (string.IsNullOrWhiteSpace(line.Name))
                return FailValidation($"Не заполнено наименование товара в позиции №{number} (FiscalLine.Name).");
            if (line.Quantity == 0)
                // Нулевое количество автоматически заменяется на единицу.
                line.Quantity = 1;
            if (line.Quantity < 0)
                return FailValidation($"Количество товара в позиции №{number} должно быть больше нуля (FiscalLine.Quantity).");
            if (line.SignMethodCalculation == null)
                return FailValidation($"Не указан признак способа расчёта в позиции №{number} (FiscalLine.SignMethodCalculaion)");
            if (line.SignCalculationObject == null)
                return FailValidation($"Не указан признак предмета расчёта в позиции №{number} (FiscalLine.SignCalculationObject)");
        }

        if (fiscalCount == 0)
            return FailValidation("В чеке нет ни одной фискальной строки");

        return true;
    }

    /// <summary>
    /// Проверка шаблона печати перед отправкой: имя шаблона и наличие хотя бы одной строки.
    /// </summary>
    private bool ValidateTemplate()
    {
        if (!ValidateTemplateName(TemplateName, "шаблона печати"))
            return false;

        if (!_positions.Any(position => position is not FiscalLine))
            return FailValidation("В шаблоне печати нет ни одной строки");

        return true;
    }

    /// <summary>
    /// Проверка шаблона чека перед отправкой: имя шаблона и товарная часть чека.
    /// </summary>
    private bool ValidateCheckTemplate(string name)
        => ValidateTemplateName(name, "шаблона чека") && ValidatePositions();

    /// <summary>
    /// Имя шаблона — уникальный идентификатор на сервере: непустое и без пробелов.
    /// </summary>
    private bool ValidateTemplateName(string name, string kind)
    {
        if (string.IsNullOrWhiteSpace(name))
            return FailValidation($"Не указано имя {kind} (TemplateName).");

        if (name.Any(char.IsWhiteSpace))
            return FailValidation($"Имя {kind} «{name}» содержит пробелы — сервер такое имя не примет.");

        return true;
    }

    /// <summary>
    /// Фиксация ошибки проверки: чек не отправляется
    /// </summary>
    private bool FailValidation(string message)
    {
        Ok = false;
        ErrorCode = ValidationErrorCode;
        ErrorDescription = message;
        return false;
    }

    /// <summary>
    /// Читает файл изображения, проверяет формат (BMP или PNG) и кодирует его в Base64
    /// Имя картинки берётся из имени файла, если не задано.
    /// Возвращает false и заполняет ошибку, если файл не найден, не читается или не является BMP/PNG.
    /// </summary>
    private bool LoadPicture(string filePath, out string base64)
    {
        base64 = "";

        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            return FailValidation($"Файл изображения не найден: «{filePath}».");

        byte[] bytes;
        try
        {
            bytes = File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            return FailValidation($"Не удалось прочитать файл изображения: {ex.Message}");
        }

        if (!IsPngOrBmp(bytes))
            return FailValidation("Файл не является изображением BMP или PNG.");

        base64 = Convert.ToBase64String(bytes);
        return true;
    }

    /// <summary>
    /// Проверяет по сигнатуре файла, что это PNG или BMP
    /// </summary>
    private static bool IsPngOrBmp(byte[] bytes)
    {
        // BMP: "BM"
        if (bytes.Length >= 2 && bytes[0] == 0x42 && bytes[1] == 0x4D)
            return true;

        return bytes.Length >= 8
            && bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47
            && bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A;
    }

    /// <summary>
    /// Касса и кассир.
    /// </summary>
    private void FillBase(CheckbaseParameters check)
    {
        check.DeviceName = DeviceName;
        check.Cashier = BuildCashier();
    }

    /// <summary>
    /// Кассир из плоских полей CashierName / CashierVatin.
    /// Если оба пусты, кассир в чек не пишется.
    /// </summary>
    private Cashier? BuildCashier()
    {
        if (string.IsNullOrEmpty(CashierName) && string.IsNullOrEmpty(CashierVatin))
            return null;

        return new Cashier
        {
            Name = CashierName,
            Vatin = CashierVatin
        };
    }

    /// <summary>
    /// Покупатель (клиент) из плоских полей Customer.
    /// Если ни одно поле не заполнено, сведения о покупателе не отправляются.
    /// </summary>
    private Customer? BuildCustomer()
    {
        if (string.IsNullOrEmpty(CustomerInfo)
            && string.IsNullOrEmpty(CustomerVatin)
            && string.IsNullOrEmpty(CustomerEmail)
            && string.IsNullOrEmpty(CustomerPhone)
            && string.IsNullOrEmpty(CustomerDateOfBirth)
            && string.IsNullOrEmpty(CustomerCitizenship)
            && string.IsNullOrEmpty(CustomerDocumentTypeCode)
            && string.IsNullOrEmpty(CustomerDocumentData)
            && string.IsNullOrEmpty(CustomerAddress))
            return null;

        return new Customer
        {
            Info = CustomerInfo,
            Vatin = CustomerVatin,
            Email = CustomerEmail,
            Phone = CustomerPhone,
            DateOfBirth = CustomerDateOfBirth,
            Citizenship = CustomerCitizenship,
            DocumentTypeCode = CustomerDocumentTypeCode,
            DocumentData = CustomerDocumentData,
            Address = CustomerAddress
        };
    }

    /// <summary>
    /// Данные агента уровня чека из плоских полей Agent.
    /// Если ни одно поле не заполнено, данные агента не отправляются.
    /// </summary>
    private Agent? BuildAgent()
    {
        if (string.IsNullOrEmpty(PayingAgentOperation)
            && (PayingAgentPhone == null || PayingAgentPhone.Length == 0)
            && (ReceivePaymentsOperatorPhone == null || ReceivePaymentsOperatorPhone.Length == 0)
            && (MoneyTransferOperatorPhone == null || MoneyTransferOperatorPhone.Length == 0)
            && string.IsNullOrEmpty(MoneyTransferOperatorName)
            && string.IsNullOrEmpty(MoneyTransferOperatorAddress)
            && string.IsNullOrEmpty(MoneyTransferOperatorVatin))
            return null;

        return new Agent
        {
            PayingAgentOperation = PayingAgentOperation,
            PayingAgentPhone = PayingAgentPhone,
            ReceivePaymentsOperatorPhone = ReceivePaymentsOperatorPhone,
            MoneyTransferOperatorPhone = MoneyTransferOperatorPhone,
            MoneyTransferOperatorName = MoneyTransferOperatorName,
            MoneyTransferOperatorAddress = MoneyTransferOperatorAddress,
            MoneyTransferOperatorVatin = MoneyTransferOperatorVatin
        };
    }

    /// <summary>
    /// Суммы оплаты чека из плоских полей Cash / ElectronicPayment / AdvancePayment / Credit / CashProvision.
    /// Если все суммы нулевые — оплаты не отправляются.
    /// </summary>
    private Payments? BuildPayments()
    {
        if (Cash == 0 && ElectronicPayment == 0 && AdvancePayment == 0 && Credit == 0 && CashProvision == 0)
            return null;

        return new Payments
        {
            Cash = Cash,
            ElectronicPayment = ElectronicPayment,
            AdvancePayment = AdvancePayment,
            Credit = Credit,
            CashProvision = CashProvision
        };
    }

    /// <summary>
    /// Детализация безналичной оплаты (wire ElectronicPaymentInfo) из плоских полей.
    /// Одна оплата на чек; если сумма 0 и остальные поля пусты — детализация не отправляется.
    /// </summary>
    private List<ElectronicPayment>? BuildElectronicPaymentInfo()
    {
        if (ElectronicPayments.Count > 0)
            return ElectronicPayments;

        if (ElectronicPaymentAmount == 0
            && string.IsNullOrEmpty(ElectronicPaymentIdentifiers)
            && string.IsNullOrEmpty(ElectronicPaymentAdditionalInformation))
            return null;

        return new List<ElectronicPayment>
        {
            new ElectronicPayment
            {
                Amount = ElectronicPaymentAmount,
                PaymentMethod = ElectronicPaymentMethod,
                Identifiers = string.IsNullOrEmpty(ElectronicPaymentIdentifiers) ? null : ElectronicPaymentIdentifiers,
                AdditionalInformation = string.IsNullOrEmpty(ElectronicPaymentAdditionalInformation) ? null : ElectronicPaymentAdditionalInformation
            }
        };
    }

    /// <summary>
    /// Данные поставщика уровня чека из плоских полей Vendor.
    /// Если ни одно поле не заполнено, данные поставщика не отправляются.
    /// </summary>
    private Vendor? BuildVendor()
    {
        if (string.IsNullOrEmpty(VendorName)
            && string.IsNullOrEmpty(VendorVatin)
            && (VendorPhones == null || VendorPhones.Length == 0))
            return null;

        return new Vendor
        {
            Name = VendorName,
            Phones = VendorPhones,
            Vatin = VendorVatin
        };
    }

    /// <summary>
    /// Отраслевой реквизит чека из плоских полей Industry.
    /// Если ни одно поле не заполнено, реквизит не отправляется.
    /// </summary>
    private Industry? BuildIndustry()
    {
        if (string.IsNullOrEmpty(IndustryIdentifierFoiv)
            && string.IsNullOrEmpty(IndustryAttributeDocumentDate)
            && string.IsNullOrEmpty(IndustryAttributeDocumentNumber)
            && string.IsNullOrEmpty(IndustryAttributeValue))
            return null;

        return new Industry
        {
            IdentifierFoiv = IndustryIdentifierFoiv,
            DocumentDate = IndustryAttributeDocumentDate,
            DocumentNumber = IndustryAttributeDocumentNumber,
            AttributeValue = IndustryAttributeValue
        };
    }

    /// <summary>
    /// Дополнительный реквизит пользователя из плоских полей UserAttribute.
    /// Если оба поля пусты, реквизит не отправляется.
    /// </summary>
    private UserAttribute? BuildUserAttribute()
    {
        if (string.IsNullOrEmpty(UserAttributeName) && string.IsNullOrEmpty(UserAttributeValue))
            return null;

        return new UserAttribute
        {
            Name = UserAttributeName,
            Value = UserAttributeValue
        };
    }

    /// <summary>
    /// Операционный реквизит чека из плоских полей OperationalAttribute.
    /// Если ни одно поле не заполнено, реквизит не отправляется.
    /// </summary>
    private OperationalAttribute? BuildOperationalAttribute()
    {
        if (string.IsNullOrEmpty(OperationalAttributeDateTime)
            && string.IsNullOrEmpty(OperationalAttributeData)
            && !OperationalAttributeOperationId.HasValue)
            return null;

        return new OperationalAttribute
        {
            DateTime = OperationalAttributeDateTime,
            OperationId = OperationalAttributeOperationId,
            OperationData = OperationalAttributeData
        };
    }

    /// <summary>
    /// Смена, X/Z - отчёт, отчёт о расчётах, денежный ящик.
    /// </summary>
    private CheckbaseParameters CheckBase()
    {
        var check = new CheckbaseParameters();
        FillBase(check);
        return check;
    }

    /// <summary>
    /// Обычный чек.
    /// </summary>
    private CheckParameters CheckBody()
    {
        var check = new CheckParameters();
        FillCheck(check);
        return check;
    }

    /// <summary>
    /// Чек коррекции ФФД 1.2.
    /// </summary>
    private Correction120Parameters Correction120Body()
    {
        var check = new Correction120Parameters { CorrectionData = BuildCorrection() };
        FillCheck(check);
        return check;
    }

    /// <summary>
    /// Данные коррекции из плоских полей Correction.
    /// </summary>
    private CorrectionData BuildCorrection()
    {
        return new CorrectionData
        {
            Type = CorrectionType,
            Description = CorrectionDescription,
            Date = CorrectionDate,
            Number = CorrectionNumber
        };
    }

    /// <summary>
    /// Заполнение полей чека
    /// </summary>
    private void FillCheck(CheckParameters check)
    {
        FillBase(check);
        check.PaymentType = (int)PaymentType;
        check.TaxVariant = (int)TaxVariant;
        check.Customer = BuildCustomer();
        check.SenderEmail = SenderEmail;
        check.SaleAddress = SaleAddress;
        check.SaleLocation = SaleLocation;
        check.AgentSign = AgentSign.HasValue ? (int)AgentSign.Value : null;
        check.AgentData = BuildAgent();
        check.Vendor = BuildVendor();
        check.Positions = BuildPositions();
        check.Payments = BuildPayments();
        check.ElectronicPaymentInfo = BuildElectronicPaymentInfo();
        check.TextBefore = TextBefore;
        check.TextAfter = TextAfter;
        check.Electronically = IsElectronically;
        check.OperationalAttribute = BuildOperationalAttribute();
        check.IndustryAttribute = BuildIndustry();
        check.UserAttribute = BuildUserAttribute();
        check.TimeZone = TimeZone.HasValue ? (int)TimeZone.Value : null;
        check.OperationOnline = IsOperationOnline ? true : null;
        check.AdditionalAttribute = AdditionalAttribute;
    }

    /// <summary>
    /// Чек коррекции ФФД 1.05.
    /// </summary>
    private Correction105Parameters Correction105Body()
    {
        var check = new Correction105Parameters
        {
            CorrectionData = BuildCorrection(),
            PaymentType = (int)PaymentType,
            TaxVariant = (int)TaxVariant,
            Payments = BuildPayments(),
            SumTaxNone = CorrectionSumTaxNone,
            SumTax0 = CorrectionSumTax0,
            SumTax5 = CorrectionSumTax5,
            SumTax7 = CorrectionSumTax7,
            SumTax10 = CorrectionSumTax10,
            SumTax105 = CorrectionSumTax105,
            SumTax107 = CorrectionSumTax107,
            SumTax110 = CorrectionSumTax110,
            SumTax118 = CorrectionSumTax118,
            SumTax18 = CorrectionSumTax18,
            SumTax20 = CorrectionSumTax20,
            SumTax120 = CorrectionSumTax120,
            SumTax22 = CorrectionSumTax22,
            SumTax122 = CorrectionSumTax122,
            AdditionalAttribute = AdditionalAttribute
        };
        FillBase(check);
        return check;
    }

    /// <summary>
    /// Слип.
    /// </summary>
    private DocumentParameters SlipBody()
    {
        var check = new DocumentParameters { Positions = SlipTextParser.Parse(TextForPrint) };
        FillBase(check);
        return check;
    }

    /// <summary>
    /// Внесение / выемка.
    /// </summary>
    private CashdrawParameters CashBody()
    {
        var check = new CashdrawParameters { Sum = CashAmount };
        FillBase(check);
        return check;
    }

    /// <summary>
    /// Позиции чека в модель запроса.
    /// </summary>
    private ApiPosition[] BuildPositions()
        => ToApiPositions(_positions);

    /// <summary>
    /// Тело template: имя, тип и строки
    /// </summary>
    private TemplateRequest TemplateBody()
    {
        return new TemplateRequest
        {
            Name = TemplateName,
            Type = (int)TemplateType,
            TemplateItems = BuildPrintLines(_positions)
                .Select(line => new TemplateItem { PrintTemplateLine = line })
                .ToArray()
        };
    }

    /// <summary>
    /// Тело checkTemplate: собирается из текущего состояния чека
    /// только с именем шаблона.
    /// </summary>
    private CheckTemplateRequest CheckTemplateBody(string name)
    {
        var check = CheckBody();
        return new CheckTemplateRequest
        {
            Name = name,
            Document = new CheckTemplateDocumentRequest
            {
                PaymentType = check.PaymentType,
                TaxVariant = check.TaxVariant,
                Customer = check.Customer,
                SenderEmail = check.SenderEmail,
                SaleAddress = check.SaleAddress,
                SaleLocation = check.SaleLocation,
                Positions = check.Positions,
                // Сервер требует Payments в шаблоне чека (иначе ArgumentNullException), поэтому всегда передаём.
                Payments = check.Payments ?? new Payments(),
                ElectronicPaymentInfo = check.ElectronicPaymentInfo,
                Electronically = check.Electronically,
                OperationalAttribute = check.OperationalAttribute,
                IndustryAttribute = check.IndustryAttribute,
                UserAttribute = check.UserAttribute,
                TimeZone = check.TimeZone,
                OperationOnline = check.OperationOnline ?? false,
                AdditionalAttribute = check.AdditionalAttribute,
                CorrectionData = HasCorrectionData() ? BuildCorrection() : null
            }
        };
    }

    private bool HasCorrectionData()
        => !string.IsNullOrWhiteSpace(CorrectionDescription)
           || !string.IsNullOrWhiteSpace(CorrectionNumber)
           || CorrectionDate != default && CorrectionDate != DateTime.Today
           || CorrectionType != CorrectionTypes.Самостоятельно;

    private static ApiPosition[] ToApiPositions(IEnumerable<Position>? positions)
    {
        if (positions == null)
            return [];

        var result = new List<ApiPosition>();
        foreach (var position in positions)
            result.Add(ToApi(position));
        return result.ToArray();
    }

    /// <summary>
    /// Одна позиция чека в модель запроса по её типу.
    /// </summary>
    private static ApiPosition ToApi(Position position) => position switch
    {
        FiscalLine fiscal => new ApiPosition { FiscalString = fiscal },
        TextLine text => TextToApi(text),
        BarcodeLine barcode => new ApiPosition { Barcode = barcode },
        SeparatorLine separator => new ApiPosition { SeparatorLine = separator },
        PictureLine picture => new ApiPosition { Picture = picture },
        _ => throw new InvalidOperationException(
            $"Неизвестный тип позиции «{position.GetType().Name}». " +
            "Допустимы FiscalLine, TextLine, BarcodeLine, SeparatorLine, PictureLine.")
    };

    /// <summary>
    /// Текст с префиксом стиля линии ([dotted], [line], [line,dashed]) уходит как SeparatorLine.
    /// </summary>
    private static ApiPosition TextToApi(TextLine text)
    {
        var parsed = SlipTextParser.ParseLine(text.Text, text.Font, text.Alignment);
        return new ApiPosition
        {
            TextString = parsed.TextString,
            Barcode = parsed.Barcode,
            SeparatorLine = parsed.SeparatorLine,
            Picture = parsed.Picture
        };
    }

    /// <summary>
    /// Позиции чека в строки печатного шаблона (фискальные строки пропускаются).
    /// </summary>
    private static List<PrintTemplateLine> BuildPrintLines(IEnumerable<Position> positions)
    {
        var lines = new List<PrintTemplateLine>();
        foreach (var position in positions)
        {
            var line = ToPrintLine(position);
            if (line != null)
                lines.Add(line);
        }
        return lines;
    }

    private static PrintTemplateLine? ToPrintLine(Position position) => position switch
    {
        TextLine text => new PrintTemplateLine
        {
            Type = PrintLineType.Text,
            Line = text.Text,
            LineRight = text.LineRight,
            Font = text.Font ?? PrintFont.Normal,
            Alignment = text.Alignment ?? PrintAlignment.Left,
            Wrap = text.Wrap
        },
        SeparatorLine separator => new PrintTemplateLine
        {
            Type = PrintLineType.Separator,
            SeparatorLine = separator
        },
        BarcodeLine barcode => new PrintTemplateLine
        {
            Type = PrintLineType.Barcode,
            Alignment = barcode.Alignment ?? PrintAlignment.Center,
            Barcode = new PrintFormBarcode
            {
                Type = barcode.Type,
                Value = barcode.Barcode,
                Height = barcode.Height,
                BarWidth = barcode.BarWidth,
                PrintText = barcode.PrintText
            }
        },
        PictureLine picture => new PrintTemplateLine
        {
            Type = PrintLineType.Picture,
            // Выравнивание дублируем на уровне строки: печать и предпросмотр
            // позиционируют картинку по PrintTemplateLine.Alignment, а не по Picture.Alignment.
            Alignment = picture.Alignment switch
            {
                PictureAlignment.Left => PrintAlignment.Left,
                PictureAlignment.Right => PrintAlignment.Right,
                _ => PrintAlignment.Center
            },
            Picture = new Picture
            {
                PictureBase64 = picture.Value,
                Alignment = picture.Alignment,
                Width = picture.Width,
                Height = picture.Height
            }
        },
        _ => null
    };

    private void RestorePositionsFromPrintLines(IEnumerable<IPrintLine> lines)
    {
        _positions.Clear();
        foreach (var line in lines)
            AddPrintLine(line);
    }

    /// <summary>
    /// Нефискальная строка из ответа сервера (печатная форма, шаблон) обратно в позицию.
    /// </summary>
    private void AddPrintLine(IPrintLine line)
    {
        switch (line.Type)
        {
            case PrintLineType.Separator:
                AddSeparatorLine(line.SeparatorLine?.LineStyle ?? LineStyle.Solid);
                break;
            case PrintLineType.Barcode when line.Barcode != null:
                AddBarcode(
                    line.Barcode.Type ?? BarcodeType.QR,
                    line.Barcode.Value ?? "",
                    line.Alignment,
                    line.Barcode.Height,
                    line.Barcode.BarWidth,
                    line.Barcode.PrintText);
                break;
            case PrintLineType.Picture when line.Picture != null:
                AddPictureBase64(
                    line.Picture.PictureBase64 ?? "",
                    line.Picture.Alignment,
                    line.Picture.Width ?? 0,
                    line.Picture.Height ?? 0);
                break;
            default:
                if (!string.IsNullOrEmpty(line.LineRight))
                    AddText(line.Line ?? "", line.LineRight, line.Font);
                else
                    AddText(line.Line ?? "", line.Font, line.Alignment);
                break;
        }
    }

    private void RestoreCheckFromTemplate(CheckTemplateDocument document)
    {
        PaymentType = document.PaymentType;
        TaxVariant = document.TaxVariant;
        IsElectronically = document.Electronically;
        IsOperationOnline = document.OperationOnline;
        TimeZone = document.TimeZone;
        if (!string.IsNullOrEmpty(document.AdditionalAttribute))
            AdditionalAttribute = document.AdditionalAttribute!;
        if (!string.IsNullOrEmpty(document.SaleAddress))
            SaleAddress = document.SaleAddress!;
        if (!string.IsNullOrEmpty(document.SaleLocation))
            SaleLocation = document.SaleLocation!;
        if (!string.IsNullOrEmpty(document.SenderEmail))
            SenderEmail = document.SenderEmail!;

        if (document.Payments != null)
        {
            Cash = document.Payments.Cash;
            ElectronicPayment = document.Payments.Electronic;
            AdvancePayment = document.Payments.PrePaid;
            Credit = document.Payments.Credit;
            CashProvision = document.Payments.Barter;
        }

        if (document.CorrectionData != null)
        {
            CorrectionType = document.CorrectionData.Type;
            CorrectionDescription = document.CorrectionData.Description ?? "";
            CorrectionDate = document.CorrectionData.Date;
            CorrectionNumber = document.CorrectionData.Number ?? "";
        }

        _positions.Clear();
        foreach (var item in document.CheckItems)
        {
            // Нефискальная строка чека (текст, разделитель, штрихкод, картинка).
            if (item.PrintLine != null)
            {
                AddPrintLine(item.PrintLine);
                continue;
            }
            if (!item.IsFiscal && string.IsNullOrWhiteSpace(item.Name))
                continue;

            var line = AddPosition(
                item.Name ?? "",
                item.Quantity > 0 ? item.Quantity : 1,
                item.Sum,
                TaxRateFromValue(item.TaxValue),
                Enum.IsDefined(typeof(SignCalculationObject), item.ItemType)
                    ? (SignCalculationObject)item.ItemType
                    : null,
                Enum.IsDefined(typeof(SignMethodCalculation), item.PaymentMode)
                    ? (SignMethodCalculation)item.PaymentMode
                    : null);
            line.Price = item.Price;
            line.TaxSum = item.TaxSum;
            if (item.Department.HasValue)
                line.Department = item.Department.Value;
            if (item.ExciseAmount.HasValue)
                line.ExciseAmount = item.ExciseAmount;
            if (item.MeasureOfQuantity.HasValue && Enum.IsDefined(typeof(MeasureOfQuantity), item.MeasureOfQuantity.Value))
                line.MeasureOfQuantity = (MeasureOfQuantity)item.MeasureOfQuantity.Value;
            line.AdditionalAttribute = item.AdditionalAttribute;
            line.Marking = item.MarkingCode;
            line.ProductCode = item.ProductCode;
        }
    }

    private static TaxRate TaxRateFromValue(int value) => value switch
    {
        0 => TaxRate.Vat0,
        5 => TaxRate.Vat5,
        7 => TaxRate.Vat7,
        10 => TaxRate.Vat10,
        18 => TaxRate.Vat20,
        20 => TaxRate.Vat20,
        22 => TaxRate.Vat22,
        105 => TaxRate.Vat5_105,
        107 => TaxRate.Vat7_107,
        110 => TaxRate.Vat10_110,
        120 => TaxRate.Vat20_120,
        122 => TaxRate.Vat22_122,
        _ => TaxRate.None
    };
}
