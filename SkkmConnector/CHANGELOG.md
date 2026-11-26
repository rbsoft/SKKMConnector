# РБ-Софт: Коннектор Сервера ККМ #
Версия 1.34.42

## ИЗМЕНЕНИЯ ##

* 1.34.42
   - Рефакторинг коннектора без изменения публичного API и методов. Код разбора ответов, сборки запросов, HTTP-вызова и разбора адреса переписан простыми шагами; повторяющиеся куски вынесены в небольшие методы с понятными именами (`AddCounter`, `AddReportValues`, `TaxSum`, `IsEmpty`, `GroupId`). Вызовы, тела запросов и значения свойств остаются прежними.
   - Разбор ответов быстрее и экономнее по памяти: поле ищется сначала по точному имени, число из JSON читается без промежуточной строки. Разбор документа и Z-отчёта занимает примерно на треть меньше времени.
   - Если в одном объекте ответа два поля различаются только регистром (например, `DocId` и `docId`), берётся поле с точно совпавшим именем. Раньше брался первый по порядку.
   - Словарь подписей `ReportValues` очищен от записей, которые различались только регистром ключа. Подписи в `ReportValues` не изменились.

* 1.34.41
   - `CloseShift` заполняет `Counter` только из `overallTotals.Counters`. Корневой `Counters` и `shiftTotal.Counters` в `Counter` не читаются; из `shiftTotal` берутся только `ShiftIncome` и `ShiftOutcome`.

* 1.34.40
   - `ReportX` и `ReportSettlement` читают идентификатор операции и `outputParameters`. Счётчики, итоги смены и устройство в этих ответах сервер не присылает.
   - `OpenShift` читает идентификатор, ФН и устройство (`deviceInfo`). `outputParameters` не заполняется.
   - `CloseShift` заполняет `Counter` из `overallTotals`, итоги смены (`ShiftIncome`, `ShiftOutcome`) из `shiftTotal`, устройство из `deviceInfo`. `outputParameters` не заполняется.
   - `PrintCheck`, `PrintCheckCorrection120` и `PrintCheckCorrection105` не читают `deviceInfo` и `KktLicenses`.
   - `RequestKM` заполняет связь с ОИСМ, проверку формата, проверку кода, результат проверки, код и описание. `Barcode` и `CodeMarkInfo` из ответа не читаются. Свойство `MarkingBarcode`, которое нигде не заполнялось, удалено.
   - `GetStatus` берёт код ошибки устройства из поля `ErrorCode`.
   - `GetTaskStatus` заполняет имя ККТ и кассира из ответа.
   - Сумма безналичных в `Counter` читается из `Payments.Electronically`.
   - Скидка и надбавка коррекции в `Counter` заменяют скидку и надбавку операции, если блок коррекции их прислал.
   - Пустой `GetDeviceListByPool` возвращает код 400.
   - `ListIsProcessed` заменён на `IsProcessed` (`bool`): пустое значение в `Get…List` не передаёт `isProcessed`, `true` / `false` — фильтр. То же свойство заполняется из ответа документа.

   - В чеке коррекции ФФД 1.05 незаданные суммы `SumTax…` не передаются в JSON.
   - В `GoodCodeData` поле переименовано в `GTIN`.

* 1.34.26
   - В ответах документов заполняется `IsProcessed`.
   - В методах `Get…List` параметры `from` и `to` необязательны; `shift` передаётся при `ShiftNumber` > 0, `isProcessed` — при заданном `ListIsProcessed`.
   - `Clear()` сбрасывает `FromDate`, `ToDate` и `ListIsProcessed`.

* 1.34.25
   - Добавлен метод `GetCashOutList` (`GET` `/cashout/list`) — список выемок по кассе выводится в `ReportValues`.

* 1.34.24
   - В списках чеков (`GetCheckList`, `GetCorrection120List`, `GetCorrection105List`) в `ReportValues` выводятся заголовок `Чек N` и поля элемента: `DocId`, `TaxType`, `Sum`, `ShiftNumber`, `DocNumber`, `DocNumberInShift`, `ResultCode`, `Processed`.

* 1.34.21
   - Добавлен метод `GetCashInList` (`GET` `/cashin/list`) — список внесений по кассе выводится в `ReportValues`.

* 1.34.20
   - `GetSlip` заполняет свойства документа: шапку, данные ККТ и кассира из `DocumentHeader.Cashier`. Строки печати (`Lines`) выводятся в `ReportValues`.

* 1.34.19
   - Изменён формат ключа в `ReportValues` для всех ответов: индекс массива в квадратных скобках, части через пробел — `[0] CheckItems [0] MeasureOfQuantity`, `[0] TrustedInFn`. Содержимое — весь JSON ответа.

* 1.34.18
   - В `ReportValues` возвращён формат ключей `_0_DocNumber`, `_0_DocumentHeader_OrganizationInfo`, `_0_Lines_0_Barcode_PictureBase64`. Выводится весь JSON ответа, включая картинки штрихкода. Формат значений не изменился (`True`/`False`, перечисления — именем элемента).
   - Разбор ответа списка выполняется по исходному JSON без предварительного копирования через `JsonElement.Clone`.

* 1.34.17
   - В списках документов введена короткая выборка полей (в 1.34.18 возвращён полный JSON).

* 1.34.16
   - Изменён формат ключа в `ReportValues`: индекс массива в квадратных скобках, части через пробел — `[0] CheckItems [0] MeasureOfQuantity`, `[0] TrustedInFn`.

* 1.34.15
   - В списках документов (`GetOpenShiftList`, `GetCloseShiftList`, `GetReportXList`, `GetReportSettlementList`, `GetCheckList`, `GetCorrection120List`, `GetCorrection105List`, `GetSlipList`) в `ReportValues` снова выводится весь JSON ответа. Ключ элемента массива — `_0`, вложенное поле — `_0_CheckItems_0_Name`. Короткая выборка `[0] DocId` убрана.

* 1.34.14
   - В `MarkingPlannedStatus` добавлены значения: штучный товар в стадии реализации (`PieceInSale`, 5) и мерный товар реализован (`MeasuredSold`, 6).
   - В `DeviceType` добавлены значения: ККТ-Лаб (`KktLab`, 6) и Торговый баланс (`TorgBalance`, 7).

* 1.34.13
   - Доработан разбор ответов маркировки в свойства коннектора:
     - `RequestKM` — связь с ОИСМ, проверка формата, результат проверки ФН, штрихкод и `CodeMarkInfo`.
     - `GetProcessingKMResult` — код запроса, итог, коды 2106/2109/2105, статус запроса и `CodeMarkInfo`.
     - `VerifyMarking`, `VerifyMarkingTsPiot` и `VerifyMarkingLmcz` — код и описание сервиса, модуль и список `MarkingCodes`.

* 1.34.12
   - При ответе сервера с кодом, отличным от 200, в `ErrorDescription` записывается текст сервера. Для отключённой кассы это «Не удалось получить сменные итоги: Нет связи», для JSON-ошибки — поле `Description`. Если текста в теле нет, остаётся адрес и код HTTP.

* 1.34.11
   - `GetQueueTask` записывает в `ReportValues` девять полей статуса: `DocId`, `DocState`, `QueueState`, `NumberInQueue`, `ResultCode`, `ResultDescription`, `Date`, `FiscalSign`, `PrintStatusDescription`.

* 1.34.10
   - `GetCheck`, `GetCorrection120` и `GetCorrection105` заполняют из ответа сервера кассира, СНО, оплаты и суммы НДС. В свойства выводятся поля документа, полученные от Сервера ККМ.

* 1.34.9
   - В `ReportValues` известные перечисления записываются именем элемента: `OutputParameters_ShiftState` — `Opened`. Аналогично для `TaskType`, `SentToPrint`, `LicenseStatus`, `DeviceClass`, `TimeZone`, `Driver` и остальных перечислений коннектора.

* 1.34.8
   - `LicenseStatus` изменён на перечисление (`None`, `Ok`, `Fail`, `Blocked`) вместо числового значения.

* 1.34.7
   - Пустые `SaleAddress`, `SaleLocation`, `SenderEmail`, `TextBefore` и `TextAfter` передаются в запрос пустой строкой.
   - Поле исключается из JSON только если тег не передаётся: пустой `AdditionalAttribute`, пустые телефоны агента, пустые адрес, имя и ИНН оператора перевода, пустые дата, номер и значение отраслевого реквизита позиции, незаданные способ оплаты, идентификаторы и доп. сведения в `AddElectronicPaymentInfo`.
   - `RequestKM` снова передаёт необязательные поля со значениями по умолчанию: `false`, мера количества 0 и дробь 0/0.
   - Незаданные суммы `SumTax…` чека коррекции 1.05 передавались нулями (исправлено в 1.34.27). Незаданная мера количества позиции — 0 (штука).
   - В `AddPicture` выравнивание по умолчанию — по центру. В `AddSeparatorLine` стиль по умолчанию — сплошная линия.

* 1.34.6
   - Необязательные поля не входят в JSON, если их не задали. В `AddElectronicPaymentInfo` без способа оплаты, идентификаторов и доп. сведений в запрос уходит только `Amount`. Способ оплаты `0` — предоплата 100%; его нельзя получить, просто не указав параметр.
   - В `RequestKM` значения `NotSendToServer` и `WaitForResult` равные `false`, мера количества 0 и дробь 0/0 не передаются. Неполная дробь (задана одна часть) уходит на сервер как есть.

* 1.34.5
   - В списках документов (`GetOpenShiftList`, `GetCloseShiftList`, `GetReportXList`, `GetReportSettlementList`, `GetCheckList`, `GetCorrection120List`, `GetCorrection105List`, `GetSlipList`) в `ReportValues` выводятся только основные поля каждого документа с ключом `[номер] Имя` (`[0] ShiftNumber`) и описанием. Имена полей: `DocId`, `TaskType`, `ServerTime`, `FiscalDatetime`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `FiscalSign`, `FnSerialNumber`, `RnNumber`, `OrganizationInfo`, `FnFFDVersion`, `FnSaleAddress`, `FnSaleLocation`, `DeviceName`, `TerminalId`, `PoolId`, `ResultCode`, `ResultDescription`, `IsProcessed`. Строки документа, данные ККТ и прочие вложенные блоки не выводятся.
   - Удалена таблица `Documents`.

* 1.34.3
   - В `AddElectronicPaymentInfo` каждая оплата уходит со всеми четырьмя полями. Способ оплаты без значения — `0`, пустые `Identifiers` и `AdditionalInformation` — пустой строкой. Оплаты с суммой 0 больше не пропускаются.

* 1.34.2
   - В `RequestKM` переименованы свойства: `IsNotSendToServer` → `NotSendToServer`, `IsWaitForResult` → `WaitForResult`.
   - Дробное количество в `RequestKM` передаётся всегда, в том числе нули.

* 1.34.1
   - Заголовки `api_key` и `terminalId` уходят с каждым запросом, даже пустые. Отдельный таймаут запроса убран.
   - Если сервер ответил не 200, в `ErrorCode` записывается код HTTP, тело ответа не разбирается.
   - В ключах `ReportValues` вложенность и индексы задаются через `_` (`Backlog_DocumentsCounter`, `_0_ShiftNumber`).
   - В `Counter.Operation` значения: «Приход», «Возврат прихода», «Расход», «Возврат расхода».

* 1.34.0
   - Доработано поведение `Clear()`: очищаются данные чека, оплаты, коррекция, `ShiftNumber`, `CheckNumber`, `FiscalDatetime`, `ErrorCode`, `ErrorDescription`. Тип чека, СНО, кассир, данные маркировки, `Text`, `DocId` и прочие результаты прошлого вызова сохраняются.
   - Убрана проверка чека на стороне коннектора (`ErrorCode = -1` до отправки): проверку выполняет Сервер ККМ. Нулевое количество больше не заменяется на 1.
   - `FiscalDatetime` изменён на тип `DateTime`.
   - Если `MeasurementUnit` не задана, передаётся «шт».
   - У штрихкода убраны поля `Height`, `BarWidth`, `PrintText` — Сервер ККМ их не принимает.
   - Удалены методы и свойства: `ClearAll`, `FromTo` (период задаётся через `FromDate` / `ToDate`), `ServerDateTime` (время сервера — в `ServerTime`), колонки `Positions`: `ProductCode`, 19 колонок `GoodCode…`, `PictureWidth`, `PictureHeight`, `BarcodeValueBase64`.
   - Штрихкод в Base64 передаётся через `BarcodeValue`: коннектор определяет Base64 и отправляет значение в `ValueBase64`.
   - В `GoodCodeData` позиции поля: `GTIN`, `SerialNumber`, `CommodityGroup`, `Cis`, `Cis_Base64`, `StampType`.
   - Суммы `SumTax…` чека коррекции 1.05 сделаны необязательными (`null` по умолчанию и после `Clear()`): незаданные не передаются.
   - Блоки позиции передаются только при заполненном главном поле: данные агента — при `PayingAgentOperation`, поставщика — при `VendorDataName`, отраслевой реквизит — при `IndustryAttributeIdentifierFoiv`.
   - `Cashier` передаётся всегда (пустые `Name` / `Vatin`, если кассир не задан), `OperationOnline` — всегда (`false` по умолчанию), `Payments` — всегда, все пять сумм, в том числе нули.
   - Код маркировки позиции уходит в данные кода товара: `Cis` — в `GoodCodeData.Barcode` как есть, `Cis_Base64` — в `GoodCodeData.MarkingCode`.
   - `PrintCheck` / `PrintCheckAsync` передают `CorrectionData`, если данные коррекции заполнены.
   - `CorrectionDataType` и `CorrectionDataDate` сделаны необязательными (`null` по умолчанию и после `Clear()`); незаданные поля коррекции не передаются.
   - `RequestKM` каждый раз создаёт новый `Guid`. Заданный заранее `Guid` не используется.

* 1.33.0
   - `FnRnNumber` переименован обратно в `RnNumber`: регистрационный номер ККТ отдаётся в `RnNumber`.
   - Удалены свойства: `Timeout` (таймаут внутренний, 60 с), `Ok` (успех — результат метода или `ErrorCode = 0`), `ReportType`, `Documents` и тип `Document` (списки — в `ReportValues`), `ElectronicPaymentAmount` / `…Method` / `…Identifiers` / `…AdditionalInformation`.
   - `AddElectronicPayment` переименован в `AddElectronicPaymentInfo(amount, paymentMethod, identifiers, additionalInformation)`; все параметры, кроме суммы, необязательны, способ оплаты без значения не передаётся.
   - В `NewCheck(paymentType, taxVariant, cashierName, cashierVatin)` кассир задаётся, если передан.

* 1.32.2
   - В `PrintSlip` / `PrintSlipAsync` кассир в запрос больше не передаётся.

* 1.32.1
   - В `PrintCheckCorrection105` / `PrintCheckCorrection105Async` в запрос уходят данные покупателя (`Customer…`).

* 1.32.0
   - Переименованы свойства: `Devices` → `DeviceList`, `Counters` → `Counter`, `NotSendToServer` → `IsNotSendToServer`, `WaitForResult` → `IsWaitForResult`, `RnNumber` → `FnRnNumber`. Поля запросов и ответов сервера не изменились.

* 1.31.3
   - В `PrintCheckCorrection105` / `PrintCheckCorrection105Async` можно передать `DocId` для повторной печати: если чек с таким DocId уже есть, сервер новый не создаёт. DocId уходит только если задан явно; значение, оставшееся от прошлого ответа, не отправляется.

* 1.31.2
   - Добавлены методы для строк документа: `AddPicture(value, alignment)`, `AddBarcode(type, value, alignment)`, `AddSeparatorLine(lineStyle)`.

* 1.31.1
   - `Codes` изменён на строку с кодами маркировки через запятую: `connector.Codes = "код1,код2"`. Перед отправкой строка делится по запятым и переносам строк, пустые значения пропускаются.

* 1.31.0
   - Переработан набор свойств коннектора: оставлено 179 свойств вместо 787. Сохранены поля из описания коннектора (имена с `Is…`, `FnWarningsIs…`, `ServerDateTime`), таблицы `Positions`, `Documents`, `Counters`, `ReportValues`, `Devices`, `KktLicenses` и вспомогательные методы.
   - Удалены дублирующие и развёрнутые свойства: `Status…`, `Check…`, `ShiftTotal…` и `Overall…` (около 400), `Marking…`, `QueueTask…`, `TaskStatus…` (кроме даты постановки задания — теперь `DateTime`), `Ism…`, `License…`, `FnModes…`, `Ofd…`, `Customer…` / `Payments…` / `Qr…` из ответа, `DeviceDateTime`, `PrintForm`, а также списки `Checks`, `Slips`, `Corrections120`, `Corrections105`, `CheckItems`, `MarkingCodesInfo` и типы `Check`, `PrintLine` и др. Эти данные выводятся в таблице `ReportValues`.
   - Данные коррекции сведены к общим свойствам входа и выхода: `CorrectionType` / `CorrectionDescription` / `CorrectionDate` / `CorrectionNumber` → `CorrectionDataType` / `CorrectionDataDescription` / `CorrectionDataDate` / `CorrectionDataNumber`; `GetCorrection120` / `GetCorrection105` заполняют их из ответа.
   - `GetQueueTask` записывает идентификатор задания в `DocId`.
   - Свойства заполняются только из ответа вызванного метода. Если поля в ответе нет, свойство пустое; `TaskType`, `SentToPrint`, `DeviceClass`, `DeviceTimeZone` стали необязательными (`null`).
   - Ответ сервера разбирается один раз: поле `Result` разворачивается в таблицу `ReportValues`, блоки читаются в свойства напрямую. Модели ответа (около 30 классов) удалены.
   - Выходные свойства доступны только для чтения (`private set`), кроме `DocId`, `ShiftNumber`, `CheckNumber`, `CashAmount`.
   - Безналичная оплата задаётся плоскими свойствами: `ElectronicPaymentAmount`, `ElectronicPaymentMethod`, `ElectronicPaymentIdentifiers`, `ElectronicPaymentAdditionalInformation`. Несколько оплат — через `AddElectronicPayment(...)`. Список `ElectronicPayments` и тип `ElectronicPayment` удалены.
   - Удалены `SetCashier`, `SetTaxType` и `NewCheckCorrection`: кассир и СНО задаются свойствами `CashierName`, `CashierVatin`, `TaxVariant`; данные коррекции — свойствами `CorrectionData…` после `NewCheck`. Кассир и СНО сохраняются между запросами: `Clear` и `NewCheck` их не сбрасывают, сбрасывает только `ClearAll`.
   - Запрос собирается сразу в JSON из входных свойств (имена полей — как в API v4 Сервера ККМ). Удалены классы-контракты запроса и вспомогательные типы: `Cashier`, `Customer`, `Payments`, `UserAttribute`, `OperationalAttribute`, `CorrectionData`, `Agent`, `Vendor`, `Industry`, `FractionalQuantity`, `CommodityNomenclatureCode`, `SeparatorLine`, `BarcodePrintText`.
   - Методы, возвращающие документ, заполняют `FnSaleAddress`, `FnSaleLocation` и `ServerVersion` из ответа.
   - `ErrorCodeDescription`, `DriverType`, `DriverVersion`, `DriverVendor`, `FnSaleAddress`, `FnSaleLocation`, `FnFFDVersion` — тип `string`: нет поля в ответе — пустая строка.
   - `GetStatus` и `GetKktInfo` заполняют `CashBalance`.
   - В `OpenShift`, `CloseShift`, `ReportX`, `ReportSettlement` в `FiscalDatetime` записываются дата и время по часам ФН из ответа.
   - Методы `Get…Result` заполняют `FnWarningsIs…` из блока `Warnings` документа.
   - `KktLicenses` заполняется из данных ККТ любого ответа (`OpenShift`, `CloseShift`, методы получения документа), а не только из `GetKktInfo`.
   - `GetTaskStatus` заполняет данные из шапки документа: `OrganizationInfo`, `DeviceSerialNumber`, `FnVatin`, `RnNumber`, `FnSerialNumber`, `FnFnsUrl`, `FnOfdName`, `FnOfdVatin`.
   - Удалены запросы: шаблоны печати и шаблоны чека, картинки в ККТ, фискализация, операции, очередь (`GetQueue`, `GetQueueTaskHistory`, `CancelQueueTask` — остался `GetQueueTask`), а также `GetPrintForm`, `GetPoolList`, `RebootDevice`, `PrintCheckCopyFn`, `GetCashInList`. Вместе с ними удалены связанные свойства и типы.
   - Добавлены таблицы `Counters` (строки «приход», «возврат прихода», «расход», «возврат расхода») и `ReportValues` (поле `Result` любого ответа в виде строк `Key` / `Value` / `Description`).
   - Строки чека ведутся в таблице `Positions`. Добавлены колонки `CommodityGroup` и `MeasurementUnit`.
   - `AddPosition` и `AddText` возвращают строку `Position`. Штрихкод, картинка и разделительная линия добавляются через `Positions.Add()`; методы `AddBarcode`, `AddSeparatorLine`, `AddPicture` и `AddText(left, right)` удалены.
   - Удалены поля строки, которые сервер принимает только в шаблонах печати: `LineRight`, `Wrap`, высота, ширина штриха и печать текста штрихкода.
   - Текст фискальной строки (`Text`) печатается отдельной строкой после неё. Слип (`PrintSlip`) печатает `Text`, а затем нефискальные строки из `Positions`.
   - `CheckIsProcessed` переименован в `IsProcessed`. Входное `IsProcessed` удалено.
   - Переименованы методы: `GetCheckCorrection120` → `GetCorrection120`, `GetCheckCorrection120List` → `GetCorrection120List`, `GetCheckCorrection105` → `GetCorrection105`, `GetCheckCorrection105List` → `GetCorrection105List`, `PrintLastCheckCopy` → `PrintCheckCopyLast`, `GetOpenShift` → `GetOpenShiftResult`, `GetCloseShift` → `GetCloseShiftResult`, `GetReportX` → `GetReportXResult`, `GetReportSettlement` → `GetReportSettlementResult`.
   - Переименованы свойства подключения и списков: `PoolName` → `Pool`, `ShiftsFrom` / `ShiftsTo` → `FromDate` / `ToDate`, `IsFiscal` → `DeviceIsFiscal`, `FnFfdVersion` → `DeviceFnFfdVersion`, `LineLength` / `LineLengthPixels` → `DeviceLineLength` / `DeviceLineLengthPixels`.
   - Переименованы входные свойства: `DocumentId` → `DocId`, `TextForPrint` → `Text`, `MarkingQuantity` → `Quantity`, `MarkingCodes` → `Codes`, `VendorName` / `VendorVatin` / `VendorPhones` → `VendorDataName` / `VendorDataVatin` / `VendorDataPhones`, `IndustryIdentifierFoiv` → `IndustryAttributeIdentifierFoiv`, `IndustryAttributeValue` → `IndustryAttributeAttributeValue`, `OperationalAttributeData` → `OperationalAttributeOperationData`, `OperationalAttributeOperationId` → `OperationalAttributeOperationID`, `CorrectionSumTax…` → `SumTax…`.
   - Переименованы свойства ФН, ОФД и сервера: `FnNumber` → `FnSerialNumber`, `FnsUrl` → `FnFnsUrl`, `OfdName` → `FnOfdName`, `OfdVatin` → `FnOfdVatin`, `OrganizationVatin` → `FnVatin`, `ServerProduct` → `Product`, `ComputerTime` → `ServerTime`, `DeviceLicenses` → `KktLicenses`.
   - Переименованы свойства статуса ККТ: `StatusIs…` → `Is…`, `StatusTotalSum` → `TotalSum`, `StatusOpenShiftTime` → `OpenShiftDate`, `StatusErrorCodeDescription` → `ErrorCodeDescription`.
   - Переименованы свойства смены: `CurrentShiftState` → `ShiftState`, `ShiftDocumentsCount` → `NumberOfDocuments`, `FnDaysResources` → `ResourcesFn`, `ShiftTotalIncomeSum` / `ShiftTotalIncomeCount` / `ShiftTotalOutcomeSum` / `ShiftTotalOutcomeCount` → `ShiftIncomeSum` / `ShiftIncomeCount` / `ShiftOutcomeSum` / `ShiftOutcomeCount`.
   - `CheckNumberInShift` — только номер чека за смену из документа. Количество кассовых чеков за смену вынесено в `NumberOfChecks`.
   - Добавлено свойство `LastCheckNumber` — номер последнего фискального документа из выходных параметров смены и отчётов.
   - `CheckFfdVersion` переименован в `FnFFDVersion`.
   - Значения перечислений переведены на английские имена: `CorrectionTypes.Самостоятельно` / `ПоПредписанию` → `OnOwn` / `AsPrescribed`; `SignCalculationObject.АТНМ` / `АТМ` / `ТНМ` / `ТМ` → `ExcisableMarkedWithoutCode` / `ExcisableMarkedWithCode` / `MarkedWithoutCode` / `MarkedWithCode`. Числовые значения не изменились.
   - В `AddPosition` изменён порядок параметров: добавлены цена после количества и необязательная мера количества в конце.
   - `NewCheck(paymentType, taxVariant)` — один метод, тип чека и СНО обязательны; кассир задаётся свойствами `CashierName` / `CashierVatin`.
   - Добавлен файл `EXAMPLES.md` с примерами вызова всех методов.
   - Переименованы свойства: `FiscalDateTime` → `FiscalDatetime`, `TaskStatusDate` → `DateTime`, `RequestKmGuid` → `Guid`.
   - Добавлено свойство `DocumentsCounter` — количество чеков и чеков коррекции из выходных параметров документа смены.
   - Добавлена таблица `Documents` для документов из списков после методов `Get…List`.
   - `CashAmount` — вход и выход: после `CashIn`, `CashOut`, `GetCashIn`, `GetCashOut` в нём сумма операции из ответа; после других запросов — 0. `GetOverall` пишет наличные в ящике в `CashAmount`.
   - GET-методы вызываются без параметров: значение задаётся свойством перед вызовом (`DocId`, `ShiftNumber`, `CheckNumber`).
   - `DateTime` заполняется из выходных параметров смены и отчётов.
   - Сведены свойства статуса задания и документа: `TaskStatusSentToPrint` → `SentToPrint`, `TaskStatusNumberInQueue` → `NumberInQueue`, `TaskStatusQueueSize` → `QueueSize`, `CheckDrawerNumber` → `DrawerNumber`; пары свойств объединены в `TaskType`, `ResultCode`, `ResultDescription`, `PoolId`.

* 1.30.17
   - Часовая зона отдаётся перечислением `CheckTimeZone` вместо `int` в свойствах `DeviceTimeZone`, `CheckTimeZone`, `Check.TimeZone`, `Check.DeviceTimeZone`, `RelatedOperation.DeviceTimeZone`.
   - В `CheckItem` признаки расчёта позиции — только enum-свойства `SignMethodCalculation` и `SignCalculationObject`. Числовые `PaymentMode` и `ItemType` убраны.

* 1.30.16
   - В `AddPosition(name, quantity, sum, tax, signCalculationObject, signMethodCalculation)` признак предмета расчёта и признак способа расчёта стали обязательными параметрами.
   - `BarcodeLine.Barcode` переименовано в `BarcodeLine.Value`.
   - Документация `API.md` переписана в формате руководства программиста: у каждого метода таблицы «Вход» и «Выход», у каждого свойства — методы, для которых оно входное и которые его заполняют.

* 1.28.10
   - Шаблоны печати и шаблоны чека собираются так же, как обычный чек: `NewTemplate` / `NewCheckTemplate`, используют `AddText`, `AddBarcode`, `AddSeparatorLine`, `AddPicture`, `AddPosition`.
   - Добавлены перегрузки с именем в параметре: `AddCheckTemplate(name)`, `UpdateCheckTemplate(name)`, `GetTemplate(name)`, `GetCheckTemplate(name)`, `DeleteTemplate(name)`.
   - Перед отправкой проверяется: имя шаблона обязательно и без пробелов, шаблон не должен быть пустым.
   - `NewCheck` и `NewCheckCorrection` очищают предыдущий чек и ответ.
   - Данные кассира и СНО задаются на смену (`SetCashierName`, `SetCashierVatin`, `SetTaxType`) и не сбрасываются между чеками.
   - `Clear()` очищает текущий чек, `ClearAll()` — также кассира и СНО.
   - Ставка НДС позиции переведена на перечисление `TaxRate` (`Vat20`, `Vat22`, `Vat10`, `Vat0`, `Vat5`, `Vat7`, `None`, расчётные ставки `Vat20_120`, `Vat5_105`, `Vat7_107`, `Vat10_110`, `Vat22_122`).
   - GET-методы принимают значение параметром: `GetCheck(docId)`, `GetFiscalSign(номерФД)`, `GetCheckList(номерСмены)` и другие.
   - Добавлен метод `FromTo(с, по)` для задания периода списков.

* 1.25.5
   - Коннектор покрывает REST API Сервера ККМ 4: чеки, смены, отчёты, наличные, нефискальные документы, картинки, маркировка, шаблоны, очередь, операции, фискализация и администрирование.
   - Добавлена работа с печатными шаблонами:
     - `POST` `/template` — создание шаблона
     - `PUT` `/template` — изменение шаблона
     - `DELETE` `/template` — удаление шаблона
     - `GET` `/template/list` — список шаблонов
     - `GET` `/template` — получение шаблона по имени
   - Добавлена работа с шаблонами чека:
     - `POST` `/checkTemplate` — создание шаблона чека
     - `PUT` `/checkTemplate` — изменение шаблона чека
     - `DELETE` `/checkTemplate` — удаление шаблона чека
     - `GET` `/checkTemplate/list` — список шаблонов чека
     - `GET` `/checkTemplate` — получение шаблона чека по имени
     - При создании шаблона чека позиции уходят в том же виде, что и при печати чека (`FiscalString`).
   - Добавлена работа с очередью печати:
     - `GET` `/queue` — состояние очереди
     - `GET` `/queue/task` — состояние задания
     - `GET` `/queue/task/history` — история задания
     - `DELETE` `/queue/task` — отмена задания в очереди
   - Добавлены запросы для получения дополнительных данных операции:
     - `GET` `/operation/last` — последняя операция
     - `GET` `/operation` — операция по идентификатору документа
     - `GET` `/operation/history` — история выполнения операции
     - `GET` `/operation/tlv` — TLV операции
     - `GET` `/operation/km` — коды маркировки операции
     - `GET` `/operation/related` — связанные операции
     - `GET` `/operation/list` — список операций за период
   - Добавлена фискализация кассы:
     - `POST` `/fiscalization` — фискализация
     - `POST` `/fiscalization/async` — асинхронная фискализация
     - `GET` `/fiscalization` — результат фискализации по документу
     - `GET` `/fiscalization/list` — список операций фискализации
   - Добавлена проверка кодов маркировки:
     - `POST` `/marking/km/verify` — проверка КМ
     - `POST` `/marking/km/tspiot/verify` — проверка КМ через ТС ПИоТ
     - `POST` `/marking/km/lmcz/verify` — проверка КМ через ЛМ ЧЗ
   - Добавлено администрирование сервера и касс:
     - `GET` `/user/token` — получение токена по логину и паролю
     - `GET` `/user/list` — список пользователей
     - `POST` `/user` — добавление пользователя
     - `PUT` `/user` — изменение пользователя
     - `DELETE` `/user` — удаление пользователя
     - `GET` `/service/settings` — настройки службы
     - `POST` `/service/settings` — сохранение настроек службы
     - `POST` `/kkt` — добавление кассы
     - `PUT` `/kkt` — изменение кассы
     - `DELETE` `/kkt` — удаление кассы
     - `POST` `/kkt/reboot` — перезагрузка кассы
     - `POST` `/kkt/font/setting` — настройка шрифтов кассы
     - `GET` `/pool/list` — список пулов
     - `GET` `/kkt/list/byPool` — кассы пула
     - `GET` `/version` — версия сервера
   - Добавлены операции с документами и картинками:
     - `GET` `/check/list` — список чеков за период
     - `POST` `/check/copy/fn` — копия чека из ФН
     - `GET` `/slip` — получение нефискального документа
     - `GET` `/slip/list` — список нефискальных документов
     - `GET` `/picture` — получение изображения
     - `DELETE` `/picture` — удаление изображения
   - Для смен, отчётов и внесений/выемок добавлены асинхронные методы: `OpenShiftAsync`, `CloseShiftAsync`, `ReportXAsync`, `ReportSettlementAsync`, `CashInAsync`, `CashOutAsync`.
   - Период списков отчётов, чеков и операций задаётся свойствами `ShiftsFrom` и `ShiftsTo`.
   - Перечисление `CheckType` дополнено типами заданий сервера: слип, фискализация, смена, отчёты, внесение, выемка, копия из ФН, дубликат документа.
   - Транспорт поддерживает `PUT` и `DELETE`, а также Basic Auth для получения токена пользователя.
