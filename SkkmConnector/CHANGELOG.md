## ИЗМЕНЕНИЯ


* 1.34.14
  - `MarkingPlannedStatus`: штучный товар в стадии реализации (`PieceInSale`, 5) и мерный товар реализован (`MeasuredSold`, 6).
  - `DeviceType`: ККТ-Лаб (`KktLab`, 6) и Торговый баланс (`TorgBalance`, 7).

* 1.34.13
  - Ответы маркировки разбираются в свойства. `RequestKM`: связь с ОИСМ, проверка формата, результат проверки ФН, штрихкод и `CodeMarkInfo`. `GetProcessingKMResult`: код запроса, итог, коды 2106/2109/2105, статус запроса и `CodeMarkInfo`. `VerifyMarking`, `VerifyMarkingTsPiot` и `VerifyMarkingLmcz`: код и описание сервиса, модуль и список `MarkingCodes`.

* 1.34.12
  - Ответ не 200: в `ErrorDescription` текст сервера. Для отключённой кассы это «Не удалось получить сменные итоги: Нет связи», для JSON-ошибки — поле `Description`. Если текста в теле нет, остаётся адрес и код HTTP.

* 1.34.11
  - `GetQueueTask`: в `ReportValues` девять полей статуса (`DocId`, `DocState`, `QueueState`, `NumberInQueue`, `ResultCode`, `ResultDescription`, `Date`, `FiscalSign`, `PrintStatusDescription`). Раньше туда попадал весь JSON задания.

* 1.34.10
  - `GetCheck`, `GetCorrection120` и `GetCorrection105` заполняют из ответа сервера кассира, СНО, оплаты и суммы НДС. В выводе — поля документа, которые пришли от Сервера ККМ.

* 1.34.9
  - В `ReportValues` известные перечисления пишутся именем элемента: `OutputParameters_ShiftState` — `Opened`. Так же `TaskType`, `SentToPrint`, `LicenseStatus`, `DeviceClass`, `TimeZone`, `Driver` и остальные перечисления коннектора.

* 1.34.8
  - `LicenseStatus` — перечисление (`None`, `Ok`, `Fail`, `Blocked`), как на Сервере ККМ. Раньше было числом.

* 1.34.7
  - Пустые `SaleAddress`, `SaleLocation`, `SenderEmail`, `TextBefore` и `TextAfter` уходят в запрос пустой строкой.
  - Из JSON пропадает поле, только если тег не передаётся: пустой `AdditionalAttribute`, пустые телефоны агента, пустые адрес, имя и ИНН оператора перевода, пустые дата, номер и значение отраслевого реквизита позиции, незаданные способ оплаты, идентификаторы и доп. сведения в `AddElectronicPaymentInfo`.
  - `RequestKM` снова передаёт необязательные поля со значениями по умолчанию: `false`, мера количества 0 и дробь 0/0.
  - Незаданные суммы `SumTax…` чека коррекции 1.05 уходят нулями. Незаданная мера количества позиции — 0 (штука).
  - `AddPicture`: выравнивание по умолчанию — по центру. `AddSeparatorLine`: стиль по умолчанию — сплошная линия.

* 1.34.6
  - Необязательные поля не входят в JSON, если их не задали. `AddElectronicPaymentInfo`: без способа оплаты, идентификаторов и доп. сведений в запрос уходит только `Amount`. Способ оплаты `0` — это предоплата 100%, его больше нельзя получить, просто не указав параметр.
  - `RequestKM`: `NotSendToServer` и `WaitForResult` со значением false, мера количества 0 и дробь 0/0 не передаются. Неполная дробь (задана одна часть) уходит на сервер как есть.

* 1.34.0
  Поведение запросов
  - `Clear()` очищает данные чека, оплаты, коррекцию, `ShiftNumber`, `CheckNumber`, `FiscalDatetime`, `ErrorCode`, `ErrorDescription`. Тип чека, СНО, кассир, данные маркировки, `Text`, `DocId` и прочие результаты прошлого вызова сохраняются.
  - Убрана проверка чека на стороне коннектора (`ErrorCode = -1` до отправки): чек проверяет Сервер ККМ. Нулевое количество больше не заменяется на 1.
  - `FiscalDatetime` — дата (`DateTime`). Раньше была строкой.
  - `MeasurementUnit`: если не задана, передаётся «шт».
  - Штрихкод: убраны поля `Height`, `BarWidth`, `PrintText` — Сервер ККМ их не принимает.
  - Удалены лишние методы и свойства (ломающее изменение): `ClearAll`, `FromTo` (период задаётся через `FromDate` / `ToDate`), `ServerDateTime` (время сервера теперь в `ServerTime`), колонки `Positions`: `ProductCode`, 19 колонок `GoodCode…`, `PictureWidth`, `PictureHeight`, `BarcodeValueBase64`.
  - Штрихкод в Base64 передаётся через `BarcodeValue`: коннектор сам определяет Base64 (есть латинская буква и строка декодируется), и отправляет его в `ValueBase64`.
  - `GoodCodeData` позиции — поля: `GTIN`, `SerialNumber`, `CommodityGroup`, `Cis`, `Cis_Base64`, `StampType`.
  - Суммы `SumTax…` чека коррекции 1.05 стали необязательными (`null` по умолчанию и после `Clear()`): незаданные не передаются. Раньше уходили нулями.
  - Блоки позиции передаются только при заполненном главном поле: данные агента — при `PayingAgentOperation`, поставщика — при `VendorDataName`, отраслевой реквизит — при `IndustryAttributeIdentifierFoiv`.
  - `Cashier` передаётся всегда (пустые `Name` / `Vatin`, если кассир не задан), `OperationOnline` — всегда (`false` по умолчанию), `Payments` — всегда, все пять сумм, в том числе нули.
  - Код маркировки позиции уходит в данные кода товара: `Cis` — в `GoodCodeData.Barcode` как есть, `Cis_Base64` — в `GoodCodeData.MarkingCode`. Раньше оба уходили в `MarkingCode` позиции, а `Cis` кодировался в Base64.
  - `PrintCheck` / `PrintCheckAsync` передают `CorrectionData`, если данные коррекции заполнены.
  - `CorrectionDataType` и `CorrectionDataDate` стали необязательными (`null` по умолчанию и после `Clear()`); незаданные поля коррекции не передаются.
  - `RequestKM` каждый раз создаёт новый `Guid`. Заданный заранее `Guid` не используется.

* 1.34.5
  - Списки документов (`GetOpenShiftList`, `GetCloseShiftList`, `GetReportXList`, `GetReportSettlementList`, `GetCheckList`, `GetCorrection120List`, `GetCorrection105List`, `GetSlipList`): в `ReportValues` — только основные поля каждого документа с ключом `[номер] Имя` (`[0] ShiftNumber`) и описанием. Имена — как у свойств коннектора для одного документа: `DocId`, `TaskType`, `ServerTime`, `FiscalDatetime`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `FiscalSign`, `FnSerialNumber`, `RnNumber`, `OrganizationInfo`, `FnFFDVersion`, `FnSaleAddress`, `FnSaleLocation`, `DeviceName`, `TerminalId`, `PoolId`, `ResultCode`, `ResultDescription`, `IsProcessed`. Строки документа, данные ККТ и прочие вложенные блоки больше не выводятся.
  - Таблица `Documents` из 1.34.4 удалена.

* 1.34.3
  - `AddElectronicPaymentInfo`: каждая оплата уходит со всеми четырьмя полями. Способ оплаты без значения — `0`, пустые `Identifiers` и `AdditionalInformation` — пустой строкой. Оплаты с суммой 0 больше не пропускаются.

* 1.34.2
  RequestKM
  - `IsNotSendToServer` → `NotSendToServer`, `IsWaitForResult` → `WaitForResult`.
  - Дробное количество в `RequestKM` уходит всегда, в том числе нули.

* 1.34.1
  Сведение HTTP и таблиц
  - Заголовки `api_key` и `terminalId` уходят с каждым запросом, даже пустые. Отдельного таймаута запроса нет.
  - Если сервер ответил не 200, `ErrorCode` — код HTTP, тело ответа не разбирается.
  - Ключи `ReportValues` вложенность и индексы через `_` (`Backlog_DocumentsCounter`, `_0_ShiftNumber`).
  - `Counter.Operation`: «Приход», «Возврат прихода», «Расход», «Возврат расхода».

* 1.33.0
  Сведение полей (ломающее изменение)
  - `FnRnNumber` снова называется `RnNumber`: регистрационный номер ККТ отдаётся в `RnNumber` (переименование в 1.32.0 было ошибкой).
  - Удалены лишние свойства: `Timeout` (таймаут теперь внутренний, 60 с), `Ok` (успех — результат метода или `ErrorCode = 0`), `ReportType`, `Documents` и тип `Document` (списки — в `ReportValues`), `ElectronicPaymentAmount` / `…Method` / `…Identifiers` / `…AdditionalInformation`.
  - `AddElectronicPayment` → `AddElectronicPaymentInfo(amount, paymentMethod, identifiers, additionalInformation)`; все параметры, кроме суммы, необязательны, способ оплаты без значения не передаётся.
  - `NewCheck(paymentType, taxVariant, cashierName, cashierVatin)`: кассир задаётся, если передан.

* 1.32.2
  - `PrintSlip` / `PrintSlipAsync`: кассир в запрос больше не передаётся — в слипе он не нужен.

* 1.32.1
  - `PrintCheckCorrection105` / `PrintCheckCorrection105Async`: в запрос уходят данные покупателя (`Customer…`). Раньше они передавались только в обычном чеке.

* 1.32.0
  Имена свойств (ломающее изменение)
  - `Devices` → `DeviceList`, `Counters` → `Counter`, `NotSendToServer` → `IsNotSendToServer`, `WaitForResult` → `IsWaitForResult`, `RnNumber` → `FnRnNumber`. Поля запросов и ответов сервера не изменились.

* 1.31.3
  - `PrintCheckCorrection105` / `PrintCheckCorrection105Async`: можно передать `DocId` для повторной печати — если чек с таким DocId уже есть, сервер новый не создаёт. DocId уходит, только если задан явно: значение, оставшееся от прошлого ответа, не отправляется, поэтому два чека коррекции подряд не склеятся.

* 1.31.2
  - Новые методы для строк документа: `AddPicture`(value, alignment)` — картинка, `AddBarcode(type, value, alignment)` — штрихкод, `AddSeparatorLine(lineStyle)` — разделительная линия. Раньше такие строки заполнялись вручную через `Positions.Add()`.

* 1.31.1
  - `Codes` теперь строка с кодами маркировки через запятую, `connector.Codes = "код1,код2"` (было `List<string>` и `Codes.Add(...)`). Перед отправкой строка делится по запятым и переносам строк, пустые значения пропускаются.

* 1.31.0
  Упрощение: только поля коннектора (ломающее изменение)
  - Свойств коннектора — 179 вместо 787. Остались поля из описания коннектора (наши имена с `Is…`, `FnWarningsIs…`, `ServerDateTime` сохранены), таблицы `Positions`, `Documents`, `Counters`, `ReportValues`, `Devices`, `KktLicenses` и вспомогательные методы.
  - Удалены дублирующие и развёрнутые свойства: `Status…`, `Check…`, `ShiftTotal…` и `Overall…` (около 400), `Marking…`, `QueueTask…`, `TaskStatus…` (кроме даты постановки задания — теперь `DateTime`), `Ism…`, `License…`, `FnModes…`, `Ofd…`, `Customer…`/`Payments…`/`Qr…` из ответа, `DeviceDateTime`, `PrintForm`, а также списки `Checks`, `Slips`, `Corrections120`, `Corrections105`, `CheckItems`, `MarkingCodesInfo` и типы `Check`, `PrintLine` и др. Эти данные — в таблице `ReportValues`.
  - Данные коррекции — одни свойства для входа и выхода: `CorrectionType` / `CorrectionDescription` / `CorrectionDate` / `CorrectionNumber` → `CorrectionDataType` / `CorrectionDataDescription` / `CorrectionDataDate` / `CorrectionDataNumber`; `GetCorrection120` / `GetCorrection105` заполняют их из ответа.
  - `GetQueueTask` записывает идентификатор задания в `DocId`.
  - Свойства заполняются только из ответа вызванного метода, без подстановки из другого источника: `Counters` (строка — только для операции из ответа), `TaskType`, `PoolId`, `ResultCode`, `ResultDescription`, `OrganizationInfo`, `FnVatin`, `FnOfdName`, `FnOfdVatin`. Если поля в ответе нет, свойство пустое: `TaskType`, `SentToPrint`, `DeviceClass`, `DeviceTimeZone` стали необязательными (`null`, а не первое значение перечисления).
  - Ответ сервера разбирается один раз: поле `Result` разворачивается в таблицу `ReportValues`, а его блоки читаются в свойства напрямую. Модели ответа (около 30 классов) удалены, свойства и их значения не изменились.
  - Выходные свойства — только для чтения (`private set`), кроме `DocId`, `ShiftNumber`, `CheckNumber`, `CashAmount`: их можно задать и на вход.
  - Безналичная оплата — только плоскими свойствами, как агент и поставщик: `ElectronicPaymentAmount`, `ElectronicPaymentMethod`, `ElectronicPaymentIdentifiers`, `ElectronicPaymentAdditionalInformation` Несколько оплат — `AddElectronicPayment(amount, paymentMethod, identifiers, additionalInformation)`, все параметры обязательные; метод больше ничего не возвращает. Список `ElectronicPayments` и тип `ElectronicPayment` удалены.
  - Удалены `SetCashier`, `SetTaxType` и `NewCheckCorrection`: кассир и СНО задаются свойствами `CashierName`, `CashierVatin`, `TaxVariant` (теперь доступны на запись); данные коррекции — свойствами `CorrectionData…` после `NewCheck`. Кассир и СНО сохраняются между запросами: `Clear` и `NewCheck` их не сбрасывают, сбрасывает только `ClearAll`.
  - Запрос собирается сразу в JSON из входных свойств (имена полей — как в API v4 Сервера ККМ), тела запросов не изменились. Удалены классы-контракты запроса и публичные вспомогательные типы, которые только перекладывали поля: `Cashier`, `Customer`, `Payments`, `UserAttribute`, `OperationalAttribute`, `CorrectionData`, `Agent`, `Vendor`, `Industry`, `FractionalQuantity`, `CommodityNomenclatureCode`, `SeparatorLine`, `BarcodePrintText`. Данные задаются, как и раньше, свойствами коннектора и колонками `Positions`.
  - Методы, возвращающие документ, заполняют `FnSaleAddress`, `FnSaleLocation` и `ServerVersion` из ответа.
  - `ErrorCodeDescription`, `DriverType`, `DriverVersion`, `DriverVendor`, `FnSaleAddress`, `FnSaleLocation`, `FnFFDVersion` — `string` вместо `string?`: нет поля в ответе — пустая строка.
  - `GetStatus` и `GetKktInfo` заполняют `CashBalance` (остаток наличных из статуса ККТ).
  - `OpenShift`, `CloseShift`, `ReportX`, `ReportSettlement`: `FiscalDatetime` — дата и время по часам ФН из ответа, время из выходных параметров его больше не затирает.
  - `Get…Result` заполняют `FnWarningsIs…`: в документе сервер присылает предупреждения ФН в блоке `Warnings`.
  - `KktLicenses` заполняется из данных ККТ любого ответа (`OpenShift`, `CloseShift`, методы получения документа), а не только из `GetKktInfo`.
  - `GetTaskStatus` заполняет данные из шапки документа: `OrganizationInfo`, `DeviceSerialNumber`, `FnVatin`, `RnNumber`, `FnSerialNumber`, `FnFnsUrl`, `FnOfdName`, `FnOfdVatin`.

  Набор методов (ломающее изменение)
  - Удалены запросы: шаблоны печати (`NewTemplate`, `AddTemplate`, `UpdateTemplate`, `DeleteTemplate`, `GetTemplate`, `GetTemplateList`), шаблоны чека (`NewCheckTemplate`, `AddCheckTemplate`, `UpdateCheckTemplate`, `DeleteCheckTemplate`, `GetCheckTemplate`, `GetCheckTemplateList`), картинки в ККТ (`SendPicture`, `GetPicture`, `GetPictureList`, `DeletePicture`), фискализация (`Fiscalization`, `FiscalizationAsync`, `GetFiscalization`, `GetFiscalizationList`), операции (`GetOperation`, `GetOperationLast`, `GetOperationList`, `GetOperationHistory`, `GetOperationTlv`, `GetOperationKm`, `GetOperationRelated`), очередь (`GetQueue`, `GetQueueTaskHistory`, `CancelQueueTask` — остался `GetQueueTask`), а также `GetPrintForm`, `GetPoolList`, `RebootDevice`, `PrintCheckCopyFn`, `GetCashInList`.
  - Вместе с ними удалены их входные и выходные свойства (`TemplateName`, `TemplateType`, `PictureId`, `PictureAlignment`, `QueueTaskId`, `Fiscalization…`, `Pools`, `Pictures`, `Templates`, `CheckTemplates`, `Queue`, `Operation…`, `CashIns` и др.) и неиспользуемые типы.
  - Таблицы `Counters` (строки «приход», «возврат прихода», «расход», «возврат расхода» с колонками `Count`, `Sum`, `Cash`, `Electronically`, … — после `GetTotals`, `GetOverAll`, X/Z-отчёта) и `ReportValues` (поле `Result` любого ответа в виде строк `Key` / `Value` / `Description`).
  - Строки чека — таблица `Positions`: одна строка `Position` с колонками `FiscalString`, `Name`, `Quantity`, `PriceWithDiscount`, `SumWithDiscount`, `Tax`, `SignCalculationObject`, `SignMethodCalculation`, `MeasureOfQuantity`, `Cis`, `Cis_Base64`, `GTIN`, `SerialNumber`, `StampType`, `VendorData…`, `IndustryAttribute…`, `MoneyTransferOperatorVATIN`, `SignSubjectCalculationAgent`, `Text` / `TextFont` / `TextAlignment`, `Font` / `Alignment`, `lineStyle`, `BarcodeType` / `BarcodeValue` / `BarcodeAlignment`, `PictureValue` / `PictureAlignment` и др. Наши колонки дополнительные: `ProductCode`, `GoodCode…`, `BarcodeValueBase64`, `PictureWidth`, `PictureHeight`.
  - Новые колонки `Positions`: `CommodityGroup` (тип товара) и `MeasurementUnit` (единица измерения предмета расчёта).
  - `AddPosition` и `AddText` возвращают строку `Position`. Штрихкод, картинка и разделительная линия добавляются через `connector.Positions.Add()`; `AddBarcode`, `AddSeparatorLine`, `AddPicture` и `AddText(left, right)` удалены. Типы `FiscalLine`, `TextLine`, `BarcodeLine`, `PictureLine` больше не публичные.
  - Удалены поля строки, которые сервер принимает только в шаблонах печати: правая часть и перенос текста (`LineRight`, `Wrap`), высота, ширина штриха и печать текста штрихкода.
  - Текст фискальной строки (`Text`) печатается отдельной строкой после неё. Слип (`PrintSlip`) печатает `Text`, а затем нефискальные строки из `Positions`.
  - `CheckIsProcessed` → `IsProcessed` («Успешная операция» после `GetCheck`, `GetSlip`, `GetCashIn`, `GetCashOut`). Входное `IsProcessed` (было только для `GetOperationLast`) удалено.

  Имена (ломающее изменение: старые имена убраны)
  - Методы: `GetCheckCorrection120` → `GetCorrection120`, `GetCheckCorrection120List` → `GetCorrection120List`, `GetCheckCorrection105` → `GetCorrection105`, `GetCheckCorrection105List` → `GetCorrection105List`, `PrintLastCheckCopy` → `PrintCheckCopyLast`, `GetOpenShift` → `GetOpenShiftResult`, `GetCloseShift` → `GetCloseShiftResult`, `GetReportX` → `GetReportXResult`, `GetReportSettlement` → `GetReportSettlementResult`.
  - Подключение и списки: `PoolName` → `Pool`, `ShiftsFrom` / `ShiftsTo` → `FromDate` / `ToDate`, `IsFiscal` → `DeviceIsFiscal`, `FnFfdVersion` → `DeviceFnFfdVersion`, `LineLength` / `LineLengthPixels` → `DeviceLineLength` / `DeviceLineLengthPixels`.
  - Входные свойства: `DocumentId` → `DocId`, `TextForPrint` → `Text`, `MarkingQuantity` → `Quantity`, `MarkingCodes` → `Codes`, `VendorName` / `VendorVatin` / `VendorPhones` → `VendorDataName` / `VendorDataVatin` / `VendorDataPhones`, `IndustryIdentifierFoiv` → `IndustryAttributeIdentifierFoiv`, `IndustryAttributeValue` → `IndustryAttributeAttributeValue`, `OperationalAttributeData` → `OperationalAttributeOperationData`, `OperationalAttributeOperationId` → `OperationalAttributeOperationID`, `CorrectionSumTax…` → `SumTax…`.
  - ФН, ОФД, сервер: `FnNumber` → `FnSerialNumber`, `FnsUrl` → `FnFnsUrl`, `OfdName` → `FnOfdName`, `OfdVatin` → `FnOfdVatin`, `OrganizationVatin` → `FnVatin`, `ServerProduct` → `Product`, `ComputerTime` → `ServerTime`, `DeviceLicenses` → `KktLicenses`.
  - Статус ККТ: `StatusIs…` → `Is…` (кроме `StatusIsFnPresent` и `StatusIsFiscal` — `IsFnPresent` и `DeviceIsFiscal` уже есть), `StatusTotalSum` → `TotalSum`, `StatusOpenShiftTime` → `OpenShiftDate`, `StatusErrorCodeDescription` → `ErrorCodeDescription`.
  - Смена: `CurrentShiftState` → `ShiftState`, `ShiftDocumentsCount` → `NumberOfDocuments`, `FnDaysResources` → `ResourcesFn`, `ShiftTotalIncomeSum` / `ShiftTotalIncomeCount` / `ShiftTotalOutcomeSum` / `ShiftTotalOutcomeCount` → `ShiftIncomeSum` / `ShiftIncomeCount` / `ShiftOutcomeSum` / `ShiftOutcomeCount`.
  - `CheckNumberInShift` — теперь только номер чека за смену (из документа). Количество кассовых чеков за смену из выходных параметров смены и отчётов вынесено в новое свойство `NumberOfChecks`.
  - Новое свойство `LastCheckNumber` — номер последнего фискального документа из выходных параметров смены и отчётов. `CheckNumber` по-прежнему номер ФД документа.
  - `CheckFfdVersion` → `FnFFDVersion` — версия ФФД документа в `Get…Result`, `GetCheck` и других методах получения документа.
  - Английские имена значений перечислений: `CorrectionTypes.Самостоятельно` / `ПоПредписанию` → `OnOwn` / `AsPrescribed`; `SignCalculationObject.АТНМ` / `АТМ` / `ТНМ` / `ТМ` → `ExcisableMarkedWithoutCode` / `ExcisableMarkedWithCode` / `MarkedWithoutCode` / `MarkedWithCode`. Числовые значения не изменились.
  - `AddPosition(name, quantity, price, sum, tax, signCalculationObject, signMethodCalculation, measureOfQuantity)` — порядок параметров: добавлены цена (после количества) и необязательная мера количества в конце.
  - `NewCheck(paymentType, taxVariant)` — один метод, тип чека и СНО обязательны (вместо `NewCheck(cashierName, paymentType, taxVariant)` и `NewCheck(paymentType)`); кассир задаётся свойствами `CashierName` / `CashierVatin`.
  - Документация: добавлен `EXAMPLES.md` — примеры вызова всех методов.
  - `FiscalDateTime` → `FiscalDatetime`, `TaskStatusDate` → `DateTime`. `FiscalSign` — без изменений.
  - `RequestKmGuid` → `Guid`: код запроса проверки КМ для `RequestKM`, `GetProcessingKMResult`, `ConfirmKM`.
  - Новое свойство `DocumentsCounter` — количество чеков и чеков коррекции из выходных параметров документа смены; заполняют `GetOpenShiftResult`, `GetCloseShiftResult`, `GetReportXResult`, `GetReportSettlementResult`.
  - Новая таблица `Documents` (`Document`: `DocId`, `ShiftNumber`, `DateTime`, `DeviceName`, `TerminalId`, `ResultCode`, `ResultDescription`) — документы из списка после всех методов `Get…List` (смены, отчёты, чеки, коррекции, слипы); `ReportValues` заполняется как раньше.
  - `CashAmount` — вход и выход, после `CashIn`, `CashOut`, `GetCashIn`, `GetCashOut` в нём фактическая сумма операции из ответа (поле `Sum`), после других запросов — 0, как у `DocId`. `GetOverAll` пишет наличные в ящике в `CashAmount` (раньше — в `CashBalance`); у остальных методов остаток — в `CashBalance`.
  - GET-методы без параметров: значение задаётся свойством перед вызовом. `DocId` — для `GetOpenShiftResult`, `GetCloseShiftResult`, `GetReportXResult`, `GetReportSettlementResult`, `GetCheck`, `GetCorrection120`, `GetCorrection105`, `GetCashIn`, `GetCashOut`, `GetSlip`, `GetTaskStatus`, `GetQueueTask`; `ShiftNumber` — для `GetCheckList`; `CheckNumber` — для `GetFiscalSign`. Было: `GetOpenShiftResult(documentId)`, стало: `connector.DocId = "…"; GetOpenShiftResult();`.
  - `DateTime` заполняется и из выходных параметров смены и отчётов (дата и время формирования документа): `OpenShift`, `CloseShift`, `ReportX`, `ReportSettlement` и их `Get…Result`.
  - Статус задания и документ: `TaskStatusSentToPrint` → `SentToPrint`, `TaskStatusNumberInQueue` → `NumberInQueue`, `TaskStatusQueueSize` → `QueueSize`, `CheckDrawerNumber` → `DrawerNumber`. Пары `CheckTaskType` / `TaskStatusTaskType`, `CheckResultCode` / `TaskStatusResultCode`, `CheckResultDescription` / `TaskStatusResultDescription` и `CheckPoolId` объединены в `TaskType`, `ResultCode`, `ResultDescription`, `PoolId` — их заполняют и методы получения документа, и `GetTaskStatus`.

* 1.30.17
  Перечисления вместо чисел
  - Часовая зона отдаётся как `CheckTimeZone`, а не `int`: `DeviceTimeZone`, `CheckTimeZone`, `Check.TimeZone`, `Check.DeviceTimeZone`, `RelatedOperation.DeviceTimeZone`.
  - `CheckItem`: признаки расчёта позиции — только enum-свойства `SignMethodCalculation` и `SignCalculationObject` (читаются из `PaymentMode` и `ItemType` ответа сервера). Числовые `PaymentMode` и `ItemType` убраны.

* 1.30.16
  Позиции чека
  - `AddPosition(name, quantity, sum, tax, signCalculationObject, signMethodCalculation)` — признак предмета расчёта и признак способа расчёта стали обязательными параметрами. Без них чек и раньше не отправлялся (ошибка проверки, `ErrorCode` = `-1`), теперь это видно уже при компиляции.
  - `BarcodeLine.Barcode` переименовано в `BarcodeLine.Value` — так же, как поле `Value` в JSON и `PictureLine.Value`.

  Документация
  - `API.md` переписан в формате руководства программиста: у каждого метода таблицы «Вход» и «Выход» с отметкой обязательности, у каждого свойства — методы, для которых оно входное и которые его заполняют.

* 1.28.10
  Шаблоны
  - Шаблоны печати и шаблоны чека собираются так же, как обычный чек: `NewTemplate` / `NewCheckTemplate`, используют `AddText`, `AddBarcode`, `AddSeparatorLine`, `AddPicture`, `AddPosition`;
  - Перегрузки с именем в параметре: `AddCheckTemplate(name)`, `UpdateCheckTemplate(name)`, `GetTemplate(name)`, `GetCheckTemplate(name)`, `DeleteTemplate(name)`.
  - Проверка перед отправкой: имя шаблона обязательно и без пробелов, шаблон не должен быть пустым.

  Формирование чека
  - `NewCheck(тип операции)` / `NewCheck(имя кассир, тип операции, СНО)` и `NewCheckCorrection(тип операции, основание коррекции, описание коррекции, время коррекции, номер предписания)` / `NewCheckCorrection(кассир, СНО, тип операции, основание коррекции, описание коррекции, время коррекции, номер предписания)` очищают предыдущий чек и ответ
  - Данные кассира и СНО задаются на смену (`SetCashierName`, `SetCashierVatin`, `SetTaxType`) и не сбрасываются между чеками; 
  - `Clear()` чистит текущий чек, `ClearAll()` — чистит и кассира с СНО.
  - Ставка НДС позиции переведены enum `TaxRate` (`Vat20`, `Vat22`, `Vat10`, `Vat0`, `Vat5`, `Vat7`, `None`, расчётные ставки `Vat20_120`, `Vat5_105`, `Vat7_107`,`Vat10_110`, `Vat20_120`, `Vat22_122` )

  Запросы по идентификатору
  - GET-методы принимают значение параметром: `GetCheck(docId)`, `GetFiscalSign(номерФД)`, `GetCheckList(номерСмены)`, `GetOperation(docId)`, `GetReportX(docId)`, `GetReportZ(docId)`, `GetOpenShift(docId)`, `GetReportSettlement(docId)`, `GetCorrection120(docId)`, `GetCorrection105(docId)`, `GetTaskStatus(docId)`, `GetCheck(docId)`, `GetFiscalSign(docNumber)`, `GetPrintForm(documentId)`, `GetCashIn(docId)`, `GetCashOut(docId)`, `GetSlip(docId)`, `GetPicture(pictureId)`, `GetTemplate(name)`, `GetCheckTemplate(name)`, `GetQueueTask(taskId)`, `GetQueueTaskHistory(taskId)`, `GetFiscalization(docId)`, `GetOperation(docId)`, `GetOperationHistory(docId)`, `GetOperationTlv(docId)`, `GetOperationKm(docId)`, `GetOperationRelated(docId)`
  - `FromTo(с, по)` — задание периода для списков за период.

* 1.25.5
  - Коннектор покрывает REST API Сервера ККМ 4: чеки, смены, отчёты, наличные, нефискальные документы, картинки, маркировка, шаблоны, очередь, операции, фискализация и администрирование.
  - Добавлена работа с печатными шаблонами:
    - `POST` `/template` - создание шаблона
    - `PUT` `/template` - изменение шаблона
    - `DELETE` `/template` - удаление шаблона
    - `GET` `/template/list` - список шаблонов
    - `GET` `/template` - получение шаблона по имени
  - Добавлена работа с шаблонами чека:
    - `POST` `/checkTemplate` - создание шаблона чека
    - `PUT` `/checkTemplate` - изменение шаблона чека
    - `DELETE` `/checkTemplate` - удаление шаблона чека
    - `GET` `/checkTemplate/list` - список шаблонов чека
    - `GET` `/checkTemplate` - получение шаблона чека по имени
  - При создании шаблона чека позиции уходят в том же виде, что и при печати чека (`FiscalString`), поэтому на сервере сохраняются строки чека, а не пустой шаблон.
  - Добавлена работа с очередью печати:
    - `GET` `/queue` - состояние очереди
    - `GET` `/queue/task` - состояние задания
    - `GET` `/queue/task/history` - история задания
    - `DELETE` `/queue/task` - отмена задания в очереди
  - Добавлены запросы для получения дополнительных данных операции:
    - `GET` `/operation/last` - последняя операция
    - `GET` `/operation` - операция по идентификатору документа
    - `GET` `/operation/history` - история выполнения операции
    - `GET` `/operation/tlv` - TLV операции
    - `GET` `/operation/km` - коды маркировки операции
    - `GET` `/operation/related` - связанные операции
    - `GET` `/operation/list` - список операций за период
  - Добавлена фискализация кассы:
    - `POST` `/fiscalization` - фискализация
    - `POST` `/fiscalization/async` - асинхронная фискализация
    - `GET` `/fiscalization` - результат фискализации по документу
    - `GET` `/fiscalization/list` - список операций фискализации
  - Добавлена проверка кодов маркировки:
    - `POST` `/marking/km/verify` - проверка КМ
    - `POST` `/marking/km/tspiot/verify` - проверка КМ через ТС ПИоТ
    - `POST` `/marking/km/lmcz/verify`- проверка КМ через ЛМ ЧЗ
  - Добавлено администрирование сервера и касс:
    - `GET` `/user/token` - получение токена по логину и паролю
    - `GET` `/user/list` - список пользователей
    - `POST` `/user` - добавление пользователя
    - `PUT` `/user` - изменение пользователя
    - `DELETE` `/user` - удаление пользователя
    - `GET` `/service/settings` - настройки службы
    - `POST` `/service/settings` - сохранение настроек службы
    - `POST` `/kkt` - добавление кассы
    - `PUT` `/kkt` - изменение кассы
    - `DELETE` `/kkt` - удаление кассы
    - `POST` `/kkt/reboot` - перезагрузка кассы
    - `POST` `/kkt/font/setting` - настройка шрифтов кассы
    - `GET` `/pool/list` - список пулов
    - `GET` `/kkt/list/byPool` - кассы пула
    - `GET` `/version` - версия сервера
  - Добавлены недостающие операции с документами и картинками:
    - `GET` `/check/list` - список чеков за период
    - `POST` `/check/copy/fn` - копия чека из ФН
    - `GET` `/slip` - получение нефискального документа
    - `GET` `/slip/list` - список нефискальных документов
    - `GET` `/picture` - получение изображения
    - `DELETE` `/picture` - удаление изображения
  - Для смен, отчётов и внесений/выемок добавлены асинхронные методы: `OpenShiftAsync`, `CloseShiftAsync`, `ReportXAsync`, `ReportSettlementAsync`, `CashInAsync`, `CashOutAsync`.
  - Период списков отчётов, чеков и операций задаётся свойствами `ShiftsFrom` и `ShiftsTo`.
  - Перечисление `CheckType` дополнено типами заданий сервера: слип, фискализация, смена, отчёты, внесение, выемка, копия из ФН, дубликат документа.
  - Транспорт поддерживает `PUT` и `DELETE`, а также Basic Auth для получения токена пользователя.
