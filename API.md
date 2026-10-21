# SkkmConnector - документация по API

- Базовый путь вебсервиса: `/PrintService/api/v4`
- Транспорт: HTTP, порт по умолчанию `4398`
- Встроенная документация сервера: `http://<адрес-сервера>:8888/Doc/ApiDescription`
- Каждый метод заполняет `Ok`, `ErrorCode`, `ErrorDescription` статусом ответа сервера. В строке **Заполняет** у метода — остальные свойства; если сервер поле не прислал, свойство остаётся пустым.

---

## Связь и состояние

### `Ping()`
`GET ping`

**Заполняет:** `ServerProduct`, `ServerVersion`.

Проверка доступности сервера ККМ. Не требует передачи ключа доступа (`api_key`)

**Тело ответа**

**Result (Ping)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| product | string | - | Название продукта | "RB-Soft:Server KKM" |
| version | string | - | Версия сервера | "4.0.70.611" |

### `GetVersion()`
`GET version`

**Заполняет:** `ServerVersion`.

Получение текущей версии сервера.

**Тело ответа**

**Result (ResServerVersion)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |

### `GetDeviceList()`
`GET kkt/list`

**Заполняет:** `Devices` (`DeviceNames`, `DeviceDrivers`, `DevicePools`, `DeviceStatusDescriptions`).

Получение список зарегистрированных ККТ.

**Тело ответа**

**Result (DeviceListResponse[])**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | - | Имя устройства | "Atol" |
| Driver | int | - | Тип драйвера.  <br>1 — Shtrih;  <br>2 — 1C(4.7);  <br>3 — Atol;  <br>4 — RrElectro;  <br>5 — 1C(5.0);  <br>100 — Emulator; | 3 |
| DeviceStatusDescription | string | - | Описание статуса устройства. | "Ok" |

### `GetDeviceListByPool()`
`GET kkt/list/byPool`

**Заполняет:** `Devices`, `DeviceNames` (сервер возвращает только имена касс пула).

Получить список устройств, входящих в указанный пул. 

Пулом называется именованная группа устройств ККТ, объединённых для распределения заданий на печать между несколькими кассами.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| pool | string | + | Название пула. Если пул с указанным названием не найден или в него не включено ни одно устройство, сервер возвращает пустой массив | pool |

**Тело ответа**

**Result (Devices[])**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | - | Название устройства | "Atol", <br>"Emu", <br>"Shtrih" |

### `GetPoolList()`
`GET pool/list`

**Заполняет:** `Pools`.

Получение списка всех пулов.

**Тело ответа**

**Result (массив пулов)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string[] | - | Название пулов | "asd", <br>"dsa" |

### `GetKktInfo()`
`GET kkt`

**Заполняет:** `ServerVersion`; статус: `ShiftNumber`, `CheckNumber`, `CurrentShiftState`, `ComputerTime`, `DeviceTime`, `LineLength`, `LineLengthPixels`, `IsFnPresent`, `IsFiscal`, `Status…`, `License…`, `Ism…`, `Backlog…`, `FnWarnings…`; устройство: `Device…`, `FnFfdVersion`; ФН: `FnNumber`, `RnNumber`, `FnsUrl`, `FnValidityDate`, `Fn…`, `FnModes…`, `Ofd…`, `Driver…`.

Получение подробной информации об устройстве ККТ.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (DataKkt)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ServerVersion | string | - | Версия сервера ККМ | "4.0.70.0723" |
| Fn | [FnInfo](#fninfo) | - | Описание фискального накопителя | "Fn": {<br>"Execution": "",<br>"FnContainsKeysUpdaterServerUri": false,<br>"FiscalizationsCount": 1,<br>"FiscalizationsFree": 0,<br>"FiscalizationDocumentNumber": "1",<br>"FiscalizationDateTime": "2025-03-16T00:00:00",<br>"ReasonCode": 0,<br>"LivePhase": "fiscalMode",<br>"Version": "1.0",<br>"RnNumber": "00031415926",<br>"FnsUrl": "nalog.ru",<br>"SenderEmail": "kuznicov@mail.ru",<br>"FfdVersion": "1.2",<br>"SerialNumber": "0123123123123",<br>"OrganizationName": "ООО 'Ромашка'",<br>"Vatin": "7722345678",<br>"ValidityDate": "2027-02-15T23:36:30.0725604+08:00",<br>"SaleAddress": "г.Улан-Удэ, ул.Виноградная, д11А, офис 25",<br>"SaleLocation": "Офис",<br>"TaxVariant": 63,<br>"SignOfAgent": 127,<br>"AutomaticNumber": "",<br>"Ofd": {<br>"Name": "Тестовый ОФД",<br>"Vatin": "1234554321",<br>"Host": "ofd.example.ru",<br>"Port": 7777<br>},<br>"Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"Modes": {<br>"PrinterAutomatic": false,<br>"OfflineMode": false,<br>"ServiceSign": true,<br>"BsoSign": false,<br>"CalcOnlineSign": false,<br>"DataEncryption": true,<br>"SaleExcisableGoods": true,<br>"SignOfGambling": true,<br>"SignOfLottery": true,<br>"Pawnshop": true,<br>"Assurance": true,<br>"Marking": true,<br>"VendingMachine": true,<br>"CateringServices": true,<br>"WholesaleTrade": true,<br>"AutomaticMode": false<br>}<br>} |
| Device | [KktInfo](#kktinfo) | - | Описание ККМ | "Device": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"IsFiscal": true,<br>"LineLength": 64,<br>"DeviceClass": 4,<br>"Model": "РБ-Софт:Эмулятор ККМ",<br>"ModelName": "РБ-Софт:Эмулятор ККМ",<br>"SerialNumber": "0020260207",<br>"FirmwareVersion": "2026",<br>"ConfigurationVersion": "02.07"<br>} |
| Driver | [DriverInfo](#driverinfo) | - | Описание драйвера ККМ | "Driver": {<br>"Type": "Atol",<br>"Version": "26.01.27",<br>"Vendor": "Atol"<br>} |
| Status | [KktStatus](#kktstatus) | - | Состояние обмена с ОФД | "Status": {<br>"IsFnPresent": true,<br>"IsFnError": false,<br>"IsIsmDisconnected": false,<br>"IsOfdDisconnected": false,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"ExchangeStatusIsm": {<br>"AddressIsm": "testism.ru",<br>"PortIsm": 20,<br>"Errors": {<br>"FnCommandCode": 0,<br>"DocumentNumber": 0,<br>"LastSuccessConnectionDateTime": "0001-01-01T00:00:00",<br>"Fn": {<br>"Code": 0<br>},<br>"Network": {<br>"Code": 0<br>},<br>"Ism": {<br>"Code": 0<br>}<br>},<br>"Status": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"Warnings": {<br>"DataForSendIsEmpty": false<br>}<br>},<br>"Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"ShiftNumber": 4,<br>"CheckNumber": 22,<br>"CheckNumberByShift": 13,<br>"CashSum": 2624.89,<br>"Sum": 10588.89,<br>"IsFiscal": true,<br>"OpenShiftTime": "2026-02-15T00:18:26",<br>"IsShiftOpened": true,<br>"IsShiftExpired": false,<br>"ComputerTime": "2026-02-15T23:37:05.0357118+08:00",<br>"DeviceTime": "2026-02-15T23:37:05.0357118+08:00",<br>"IsDrawerOpened": false,<br>"IsCheckPaperPresent": true,<br>"IsControlPaperPresent": false,<br>"IsWaitContinuePrint": false,<br>"IsCoverOpened": false,<br>"IsBatteryLow": false,<br>"IsOpenDocument": false,<br>"LineLength": 42,<br>"TaskId": "00000000-0000-0000-0000-000000000000",<br>"Error": 0,<br>"IsBusy": false,<br>"ErrorCode": 0,<br>"ErrorCodeDescription": "OK",<br>"DriverMode": 0,<br>"DriverModeDescription": "",<br>"DriverAdvancedMode": 0,<br>"DriverAdvancedModeDescription": "",<br>"LicenseStatus": 0,<br>"License": {<br>"code": 0,<br>"isEndUser": false,<br>"isActivated": false,<br>"isBlocked": false,<br>"blockDate": "0001-01-01T00:00:00",<br>"date": "0001-01-01T00:00:00",<br>"expired": "0001-01-01T00:00:00",<br>"updateExpired": "0001-01-01T00:00:00",<br>"limitInstalls": 0,<br>"needObjectActivation": false,<br>"limitObjects": 0,<br>"setupTokenIndex": 0,<br>"licenseUpdated": "0001-01-01T00:00:00"<br>},<br>"LicenseUpdated": "0001-01-01T00:00:00"<br>} |

### `GetStatus()`
`GET kkt/status`

**Заполняет:** `ShiftNumber`, `CheckNumber`, `CurrentShiftState`, `ComputerTime`, `DeviceTime`, `LineLength`, `LineLengthPixels`, `IsFnPresent`, `IsFiscal`, `Status…`, `License…`, `Ism…`, `Backlog…`, `FnWarnings…`.

Получение расширенного статуса ККТ.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (KktStatus)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| IsFnPresent | bool | - | Присутствует ли фискальный накопитель | false |
| IsFnError | bool | - | Находится ли фискальный накопитель в состоянии ошибки | false |
| IsIsmDisconnected | bool | - | Доступен ли информационной системы маркировки | false |
| IsOfdDisconnected | bool | - | Доступен ли оператор информационной системы маркировки | false |
| Ism | [ExchangeStatusIsm](#exchangestatusism) | - | Состояние обмена с ИСМ | "Ism": {<br>"Address": "ism.example.ru",<br>"Port": 8888,<br>"Errors": {<br>"FnCommandCode": 0,<br>"DocumentNumber": 0,<br>"LastSuccessConnectionDateTime": "0001-01-01T00:00:00",<br>"Fn": { "Code": 0 },<br>"Network": { "Code": 0 },<br>"Ism": { "Code": 0, "Description": "" }<br>},<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>}<br>} |
| ShiftNumber | int | + | Смена | 4 |
| DocNumber | int | + | Номер чека | 22 |
| CashSum | decimal | + | Сумма наличных в денежном ящике | 2624.89 |
| TotalSum | decimal | - | Сумма выручки | 110.0 |
| IsFiscal | bool | - | Фискальный режим | true |
| OpenShiftTime | DateTime | - | Время открытия кассовой смены | "2026-05-23T19:50:04.7531056+08:00" |
| IsShiftOpened | bool | - | Смена открыта | false |
| IsShiftExpired | bool | - | Смена истекла | false |
| ComputerTime | DateTime | - | Время получения данных | "2026-05-23T19:50:04.7531056+08:00" |
| DeviceTime | DateTime | - | Время в часах устройства | "2026-05-23T19:50:04.7531056+08:00" |
| IsDrawerOpened | bool | - | Открыт денежный ящик | false |
| IsCheckPaperPresent | bool | - | Наличие чековой ленты | false |
| IsControlPaperPresent | bool |- |  Наличие контрольной ленты | false |
| IsWaitContinuePrint | bool | - | Ожидание продолжения печати | false |
| IsCoverOpened | bool | - | Открыта ли крышка | false |
| IsBatteryLow | bool | - | Аккумулятор разряжен | false |
| IsOpenDocument | bool | - | - | Открытый документ | false |
| LineLength | int | - | Ширина чековой ленты | 42 |
| LineLengthPixels | int | - | Ширина чековой ленты в пикселях | 512 |
| TaskId | string | - | Идентификатор текущей задачи | "24f4bffe-98ef-4627-846c-b1f74c5a495b" |
| Error | int | - | Код ошибки | 0 |
| IsBusy | bool | - | Признак занятости устройства | false |
| ErrorCode | int | - | Код ошибки | 0 |
| ErrorCodeDescription | string | - | Описание ошибки устройства по данным драйвера | "OK" |
| DriverMode | int | - | Режим по данным драйвера | 0 |
| DriverModeDescription | string | - | Описание режима Mode по данным драйвера | "" |
| DriverAdvancedMode | int | - | Специальный режим по данным драйвера | 0 |
| DriverAdvancedModeDescription | string | - | Описание режима AdvancedMode по данным драйвера | 0 |
| LicenseStatus | int | - | Статус состояния лицензии | 0 |
| LicenseUpdated | DateTime | - | Время последней проверке лицензии | "0001-01-01T00:00:00" |
| Warnings | [Warnings](#warnings--fnwarnings) | - | Предупреждения ФН | "Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>} |
| License | [License](#license) | - | Описание лицензии | "License": {<br>"code": 0,<br>"isEndUser": false,<br>"isActivated": false,<br>"isBlocked": false,<br>"blockDate": "0001-01-01T00:00:00",<br>"date": "0001-01-01T00:00:00",<br>"expired": "0001-01-01T00:00:00",<br>"updateExpired": "0001-01-01T00:00:00",<br>"limitInstalls": 0,<br>"needObjectActivation": false,<br>"limitObjects": 0,<br>"setupTokenIndex": 0,<br>"licenseUpdated": "0001-01-01T00:00:00"<br>} |

### `GetShiftStatus()`
`GET kkt/shift/status`

**Заполняет:** `ShiftNumber`, `CheckNumber`, `CurrentShiftState`, `Backlog…`, `ShiftStatus…`.

Получение краткого статуса смены и очереди ОФД.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (ResponseCurrentStatus)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ShiftNumber | int | + | Номер смены | 4 |
| CheckNumber | int | + | Номер последнего фискального документа | 22 |
| ShiftState | int | - | Состояние смены: <br>1 — закрыта, <br>2 — открыта, <br>3 — истекла | 2 |
| Backlog | [Backlog](#backlog) | - | Статус обмена данными с ОФД | "Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>} |

### `GetLineLength()`
`GET kkt/lineLength`

**Заполняет:** `LineLength`, `LineLengthPixels`.

Получение максимальной ширины строки чека устройства.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (LineLength)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| LineLength | int | - | Ширина чековой ленты в символах | 42 |
| LineLengthPixels | int | - | Ширина чековой ленты в пикселях | 384 |

### `GetTotals()`
`GET kkt/counters/shift`

**Заполняет:** `ShiftTotal…`, `ShiftNumber`, `CashBalance`.

Получение счётчики за смену.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (ResShiftTotal)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ShiftNumber | double | - | Номер смены | 40.0 |
| CashDrawer | [CashDrawer](#cashdrawer) | - | Детали денежного ящиика | "CashDrawer": {<br>"Sum": 345.00,<br>"Count": 37<br>} |
| ShiftIncome | [ShiftIncome](#shiftincome) | - | Внесение | "ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>} |
| ShiftOutcome | [ShiftIncome](#shiftincome) | - | Выемка | "ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>} |
| Counters | [ShiftCounters](#shiftcounters) | - | Счётчики за кассовую смену | "Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 1,<br>"Sum": 50,<br>"Payments": {<br>"Sum": 0,<br>"Cash": 50<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {<br>"TaxVat_5": 2.38<br>}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>} |

### `GetOverAll()`
`GET kkt/counters/overall`

**Заполняет:** `Overall…`, `NonZeroSum`, `CashBalance`.

Коннектор кладёт `Result.Counters.Sales.Sum` в свойство `NonZeroSum`.

Получение необнуляемых (накопительных) счетчиков ККТ.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (OverallTotals)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DataLoaded | bool | - | Все ли данные успешно прочитаны из устройства | true |
| Sum | decimal | - | Сумма | 0.0 |
| Count | int | - | Количество | 0 |
| Counters | [ShiftCounters](#shiftcounters) | - | Счётчики | "Counters": {<br>"SumCorrection": 0.0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 266,<br>"Sum": 24739.54,<br>"Payments": {<br>"Sum": 24739.54,<br>"Cash": 25163.08,<br>"Credit": 9400.00,<br>"Electronically": 4949.54,<br>"Barter": 2350.00,<br>"Prepaid": 4700.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 93.03,<br>"TaxVat_7": 1536.90<br>}<br>},<br>"SalesReturn": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"SalesCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"SalesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"Purchases": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesReturn": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}<br>} |
| CashDrawer | [CashDrawer](#cashdrawer) | - | Детали денежного ящиика | "CashDrawer": {<br>"Sum": 345.00,<br>"Count": 37<br>} |

---

## Смена и отчёты

### `OpenShift()`
`POST shift/open`

**Заполняет:** `DocumentId`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`.

Открытие кассовой смены.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| Cashier | [Cashier](#cashier)          | –  | Сведения о кассире (продавце). <br><br>Если тип данных **Cashier** отсутствует или все его поля пустые, данные о кассире не передаются в чек. | {"Name": "Иванов А.И.", "Vatin": "7722345678"} |

**Тело ответа**

**Result (OpenShiftResult)**

| **Имя поля** | **Тип** | **Обяз** |**Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| datetime | datetime | - | Время операции | "2026-05-12T18:38:52" |
| deviceName | string | + | Имя устройства | "Atol" |
| docId | string | + | Идентификатор документа открытия смены. Передайте в [GET shift/open](#getopenshift), чтобы получить результат открытия смены. | "31f9d8a2-6424-4085-807c-a349d51884b1" |
| fnsUrl | string | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru" |
| fnNumber | string | - | Номер фискального накопителя | "0123123123123" |
| rnNumber | string | - | Регистрационный номер ККт | "00031415926" |
| fiscalDatetime | string | - | Дата и время документа по часам ФН | "20260523234515" |
| shiftNumber | int | + | Номер смены | 2 |
| outputParameters | [OutputParametersV4](#outputparametersv4) | - | Выходные параметры для документов | "outputParameters": {<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 5,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T00:11:23.5515451+08:00",<br>"ShiftState": 2,<br>"CashBalance": 0,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"FnValidityDate": "2027-05-24T00:11:23.5515451+08:00"<br>} |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.0.5",<br>"FnFfdVersion": "1.0.5",<br>"TimeZone": 7,<br>"Licenses": [],<br>"IsFiscal": true,<br>"LineLength": 42,<br>"LineLengthPixels": 384,<br>"DeviceClass": 4,<br>"Model": "АТОЛ 1Ф",<br>"SerialNumber": "00109325182732",<br>"FirmwareVersion": "5.8.1",<br>"ConfigurationVersion": "5.8.17"<br>} |

### `OpenShiftAsync()`
`POST shift/open/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Открытие кассовой смены асинхронно.

**Тело запроса**
Смотрите документацию [POST shift/open](#openshift)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- |
| Result | string | + | Идентификатор документа открытия смены. Передайте в [GET shift/open](#getopenshift), чтобы получить результат открытия смены. | "321581c0-2ebe-4f4e-bdf8-a932ad758dac" |

### `CloseShift()`
`POST shift/z`

**Заполняет:** `DocumentId`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`; `ShiftTotal…`, `Overall…`.

Закрытие кассовой смены.

**Тело запроса**
Смотрите документацию [POST shift/open](#openshift)

**Тело ответа**

**Result (ReportResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| datetime | datetime | - | Время операции | "2026-05-12T18:38:52" |
| deviceName | string | + | Имя устройства | "Atol" |
| docId | string | + | Идентификатор документа Z-отчёта. Передайте в [GET shift/z](#getcloseshift), чтобы получить Z-отчёт. | "31f9d8a2-6424-4085-807c-a349d51884b1" |
| fnsUrl | string | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru" |
| fnNumber | string | - | Номер фискального накопителя | "0123123123123" |
| rnNumber | string | - | РНМ | "00031415926" |
| fiscalDatetime | string | - | Дата и время документа по часам ФН | "20260523234515" |
| shiftNumber | int | - | Номер смены | 3 |
| outputParameters | [OutputParametersV4](#outputparametersv4) | - | Выходные параметры для документов | "outputParameters": {<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 6,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T01:29:32.8151052+08:00",<br>"ShiftState": 1,<br>"CashBalance": 0,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"FnValidityDate": "2027-05-24T01:29:32.8151052+08:00"<br>} |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "deviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 512,<br>"DeviceClass": 4,<br>"Model": "РБ-Софт:Эмулятор ККМ",<br>"SerialNumber": "0020260207",<br>"FirmwareVersion": "2026",<br>"ConfigurationVersion": "02.07"<br>} |
| shiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги | "shiftTotal": {<br>"IsCountersReaded": true,<br>"ShiftNumber": 3,<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>},<br>"ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0<br>}<br>} |
| OverallTotals | [OverallTotals](#overalltotals) | - | Необнуляемые / сменные итоги | "overallTotals": {<br>"DataLoaded": true,<br>"Sum": 0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}<br>} |

### `CloseShiftAsync()`
`POST shift/z/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Закрытие кассовой смены асинхронно.

**Тело запроса**
Смотрите документацию [POST shift/open](#openshift)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа Z-отчёта. Передайте в [GET shift/z](#getcloseshift), чтобы получить Z-отчёт. | "321581c0-2ebe-4f4e-bdf8-a932ad758dac" |

### `ReportX()`
`POST shift/x`

**Заполняет:** `DocumentId`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`; `ShiftTotal…`, `Overall…`.

Формирование X-отчёта (без закрытия смены)

**Тело запроса**
Смотрите документацию [POST shift/open](#openshift)

**Тело ответа**

**Result (ReportResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| datetime | datetime | - | Время операции | "2026-05-12T18:38:52" |
| deviceName | string | + | Имя устройства | "Atol" |
| docId | string | + | Идентификатор документа  X-отчёта. Передайте в [GET shift/x](#getreportx), чтобы получить X-отчёт. | "4b161ba3-bb5f-4209-964b-3c3f0e6f43b8" |
| fnsUrl | string | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru" |
| fnNumber | string | - | Номер фискального накопителя | "0123123123123" |
| rnNumber | string | - | РНМ | "00031415926" |
| fiscalDatetime | string | - | Дата и время документа по часам ФН | "20260523234515" |
| shiftNumber | int | + | Номер смены | 2 |
| outputParameters | - | OutputParametersV4 | Выходные параметры для документов | "outputParameters": {<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 6,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T01:57:54.7738055+08:00",<br>"ShiftState": 1,<br>"CashBalance": 0,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"FnValidityDate": "2027-05-24T01:57:54.7738055+08:00"<br>} |
| ShiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги | "shiftTotal": {<br>"IsCountersReaded": true,<br>"ShiftNumber": 3,<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>},<br>"ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0<br>}<br>} |
| OverallTotals | [OverallTotals](#overalltotals) | - | Необнуляемые итоги | "overallTotals": {<br>"DataLoaded": true,<br>"Sum": 0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}<br>} |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "deviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 512,<br>"DeviceClass": 4,<br>"Model": "РБ-Софт:Эмулятор ККМ",<br>"SerialNumber": "0020260207",<br>"FirmwareVersion": "2026",<br>"ConfigurationVersion": "02.07"<br>} |

### `ReportXAsync()`
`POST shift/x/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронное формирование X-отчёта.

**Тело запроса**
Смотрите документацию [POST shift/open](#openshift)

**Тело запроса**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | - | Идентификатор документа X-отчёта. Передайте в [GET shift/x](#getreportx), чтобы получить X-отчёт. | "321581c0-2ebe-4f4e-bdf8-a932ad758dac" |

### `ReportSettlement()`
`POST report/settlement`

**Заполняет:** `DocumentId`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`; `ShiftTotal…`, `Overall…`.

Формирование отчета о текущем состоянии расчетов

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| Cashier               | [Cashier](#cashier) | –        | Сведения о кассире (продавце). <br><br>Если тип данных **Cashier** отсутствует или все его поля пустые, данные о кассире не передаются в чек. | {"Name": "Иванов А.И.", "Vatin": "7722345678"} |

**Тело ответа**

**Result (ReportResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| datetime | datetime | - | Время операции | "2026-05-12T18:38:52" |
| deviceName | string | + | Имя устройства | "Atol" |
| docId | string | + | Идентификатор документа отчёта о текущем состоянии расчётов. Передайте в [GET report/settlement](#getreportsettlement), чтобы получить отчёт о состоянии расчётов. | "d4caa283-0438-4bff-8ccf-2d77a5014d7d" |
| fnsUrl | string | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru" |
| fnNumber | string | - | Номер фискального накопителя | "0123123123123" |
| rnNumber | string | - | РНМ | "00031415926" |
| fiscalDatetime | string | - | Дата и время документа по часам ФН | "20260523234515" |
| shiftNumber | int | + |Номер смены | 51 |
| outputParameters | [OutputParametersV4](#outputparametersv4) | - | Выходные параметры для документов | "outputParameters": {<br>"NumberOfChecks": 2,<br>"NumberOfDocuments": 2,<br>"ResourcesFn": 365,<br>"ShiftNumber": 51,<br>"CheckNumber": 449,<br>"ShiftClosingCheckNumber": 2,<br>"DateTime": "2026-07-27T18:51:15.4799866+08:00",<br>"ShiftState": 1,<br>"CashBalance": 2795.80,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"FnValidityDate": "2027-07-27T18:51:15.4799866+08:00"<br>} |

### `ReportSettlementAsync()`
`POST report/settlement/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Формирование отчета о текущем состоянии расчетов асинхронно

**Тело запроса**
Смотрите документацию [POST report/settlement](#reportsettlement)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа отчёта о текущем состоянии расчётов. Передайте в [GET report/settlement](#getreportsettlement), чтобы получить отчёт о состоянии расчётов. | "321581c0-2ebe-4f4e-bdf8-a932ad758dac" |

### `GetOpenShift()`
`GET shift/open`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`.

Возвращает результат открытия смены по идентификатору документа (docId), полученному в [POST shift/open](#openshift)

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа открытия смены | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (OpenShift)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| OutputParameters | [OutputParameters](#outputparameters) | - | Выходные параметры для документов | "outputParameters": {<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 5,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T00:11:23.5515451+08:00",<br>"ShiftState": 2,<br>"CashBalance": 0,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"FnValidityDate": "2027-05-24T00:11:23.5515451+08:00"<br>} |
| ShiftNumber | int | + | Номер сессии | 3 |
| DocNumber | int | + | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "5847203916" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | "" |
| Before | - | Список типа данных | Заголовок | \[\] |
| After | - | Список типа данных | Подвал | \[\] |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор документа | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| DocumentHeader | - | DocumentHeader | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "00О 'Ромашка'",<br>"SerialNumber": "00109325182732",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000001002520",<br>"Fn": "9999078902012910",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 6,<br>"DocNumber": 88,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.0.5",<br>"FnFfdVersion": "1.0.5",<br>"TimeZone": 7,<br>"Licenses": [],<br>"IsFiscal": true,<br>"LineLength": 42,<br>"LineLengthPixels": 384,<br>"DeviceClass": 4,<br>"Model": "АТОЛ 1Ф",<br>"SerialNumber": "00109325182732",<br>"FirmwareVersion": "5.8.1",<br>"ConfigurationVersion": "5.8.17"<br>} |

### `GetReportX()`
`GET shift/x`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`.

Возвращает X-отчёт по идентификатору документа (docId), полученному в [POST shift/x](#reportx).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа X-отчёта | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (ReportX)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| OutputParameters | [OutputParameters](#outputparameters) | - | Выходные параметры для документов | "OutputParameters": {<br>"DepartmentTotals": [],<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 6,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T01:57:54.7738055+08:00",<br>"ShiftState": 1,<br>"CashBalance": 0,<br>"FnValidityDate": "2027-05-24T01:57:54.7738055+08:00",<br>"DocumentsCounter": 0<br>} |
| ShiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги | "Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>} |
| OverallTotals | [OverallTotals](#overalltotals) | - | Необнуляемые итоги | "OverallTotals": {<br>"DataLoaded": true,<br>"Sum": 0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}<br>} |
| AnullatesCount | int | - | Количество аннулирваний | 0 |
| ShiftNumber | int | - | Номер сессии | 3 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "0020260207",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "00031415926",<br>"Fn": "0123123123123",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "1234554321"<br>} |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + |  Идентификатор документа X-отчёта | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "deviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 512,<br>"DeviceClass": 4,<br>"Model": "РБ-Софт:Эмулятор ККМ",<br>"SerialNumber": "0020260207",<br>"FirmwareVersion": "2026",<br>"ConfigurationVersion": "02.07"<br>} |

### `GetCloseShift()`
`GET shift/z`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`; `ShiftTotal…`, `Overall…`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`.

Возвращает Z-отчёт по идентификатору документа (docId), полученному в [POST shift/z](#closeshift).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор операции Z-отчёта | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (ReportZ)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| outputParameters | [OutputParameters](#outputparameters) | - | Выходные параметры для документов | "OutputParameters": {<br>"DepartmentTotals": [],<br>"NumberOfChecks": 1,<br>"NumberOfDocuments": 1,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"ResourcesFn": 365,<br>"ShiftNumber": 3,<br>"CheckNumber": 6,<br>"ShiftClosingCheckNumber": 1,<br>"DateTime": "2026-05-24T01:29:32.8151052+08:00",<br>"ShiftState": 1,<br>"CashBalance": 0,<br>"FnValidityDate": "2027-05-24T01:29:32.8151052+08:00",<br>"DocumentsCounter": 0<br>} |
| ShiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги | "ShiftTotal": {<br>"IsCountersReaded": true,<br>"ShiftNumber": 3,<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>},<br>"ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>}<br>} |
| OverallTotals | [OverallTotals](#overalltotals) | - | Необнуляемые итоги | "OverallTotals": {<br>"DataLoaded": true,<br>"Sum": 0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}<br>} |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "0020260207",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "00031415926",<br>"Fn": "0123123123123",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"DocNumber": 6,<br>"FiscalSign": "2691498498",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "1234554321"<br>} |
| AnullatesCount | int | - | Количество аннулирваний | 0 |
| ShiftNumber | int | - | Номер смены | 3 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "5847203916" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | "" |
| Before | - | Список типа данных | Заголовок | \[\] |
| After | - | Список типа данных | Подвал | \[\] |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор документа Z-отчёта | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 512,<br>"DeviceClass": 4,<br>"Model": "РБ-Софт:Эмулятор ККМ",<br>"SerialNumber": "0020260207",<br>"FirmwareVersion": "2026",<br>"ConfigurationVersion": "02.07"<br>} |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |

### `GetReportSettlement()`
`GET report/settlement`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`; `ShiftTotal…`, `Overall…`; выходные параметры: `CurrentShiftState`, `CheckNumber`, `CheckNumberInShift`, `ShiftDocumentsCount`, `ShiftClosingCheckNumber`, `FiscalDateTime`, `DeviceDateTime`, `CashBalance`, `FnValidityDate`, `FnDaysResources`, `Backlog…`, `FnWarnings…`.

Возвращает отчёт о состоянии расчётов по идентификатору документа (docId), полученному в [POST report/settlement](#reportsettlement).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа отчёта о текущем состоянии расчётов | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (ReportSettlements)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| outputParameters | [OutputParameters](#outputparameters) | - | Выходные параметры для документов | "DepartmentTotals": [], <br>"NumberOfChecks": 16, <br>"NumberOfDocuments": 18, <br>"Backlog": { <br>"DocumentsCounter": 0, <br>"DocumentFirstNumber": 0, <br>"DocumentFirstDateTime": "1970-01-01T00:00:00+08:00" <br>}, <br>"Warnings": { <br>"CriticalError": false, <br>"MemoryOverflow": false, <br>"NeedReplacement": false, <br>"OfdTimeout": false, <br>"ResourceExhausted": false <br>}, <br>"ResourcesFn": 422, <br>"ShiftNumber": 41, <br>"CheckNumber": 366, <br>"ShiftClosingCheckNumber": 16, <br>"DateTime": "2026-05-24T04:22:48", <br>"ShiftState": 2, <br>"CashBalance": 2165.68, <br>"FnValidityDate": "2027-07-21T00:00:00", <br>"DocumentsCounter": 0 |
| ShiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги | {"ShiftTotal": {<br>"IsCountersReaded": true,<br>"ShiftNumber": 41,<br>"CashDrawer": {<br>"Sum": 2165.68,<br>"Count": 0<br>},<br>"ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Counters": {}<br>}<br>} |
| OverallTotals | [OverallTotals](#overalltotals) | Необнуляемые итоги | {"OverallTotals": {<br>"DataLoaded": false,<br>"Sum": 0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {}<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}<br>}<br>} |
| AnullatesCount | int | - | Количество аннулирваний | 0 |
| ShiftNumber | int | + | Номер смены | 3 |
| DocNumber | int | + | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор документа отчёта о текущем состоянии расчётов | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | + | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |

### `GetCloseShiftList()`
`GET shift/z/list`

**Заполняет:** `Shifts`.

Получение списка Z-отчётов за период

**Параметры запроса**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

**Тело ответа**

**Result (ReportsResponse[])**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

### `GetOpenShiftList()`
`GET shift/open/list`

**Заполняет:** `Shifts`.

Получение списка открытий смен за период

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | Atol |
| ShiftsFrom | DateTime | + | Начало даты отбора (формат: гггг-мм-дд). | 2026-06-01 |
| ShiftsTo | DateTime | + | Конец даты отбора (формат: гггг-мм-дд). | 2026-06-30 |

**Тело ответа**

**Result (ReportsResponse[])**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| DocId | string | + | Идентификатор документа | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| ShiftNumber | int | + | Номер сессии | 3 |
| DeviceName | string | + | Имя устройства | "Atol" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |

### `GetReportXList()`
`GET shift/x/list`

**Заполняет:** `Shifts`.

Получение списка X-отчётов за период

**Параметры запроса**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

**Тело ответа**

**Result (ReportsResponse[])**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

### `GetReportSettlementList()`
`GET report/settlement/list`

**Заполняет:** `Shifts`.

Список отчётов о состоянии расчётов по устройству за период.

**Параметры запроса**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

**Тело ответа**

**Result (ReportsResponse[])**
Смотрите документацию [GET shift/open/list](#getopenshiftlist)

---

## Чеки

### `PrintCheck()`
`POST check`

**Заполняет:** `DocumentId`, `FiscalSign`, `CheckNumber`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Печать чека.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName            | string              | +        | Имя устройства. | "Atol" |
| DocId               | string              | –        | Идентификатор документа. Если указан, то сервер сначала произведет поиск чека во внутренней базе данных. В случае, если чек будет найден, считается, что задание пошло на повторную печать.  <br>  <br>Если предыдущая попытка печати была завершена успешно, то в ответ на запрос вернется результат предыдущей печати.  <br>Повторного исполнения задания на ККМ не будет. Это защита от ошибочной повторной регистрации чеков. Такие случае могут возникать в случае обрыва сетевой связи с клиентом во время обработки запроса на печать.  <br>  <br>Если же предыдущая попытка печати была неудачной, то задание будет перезаписано и проведена печать чека на ККМ.  <br>  <br>Если идентификатор документа не указан, то будет создан новый идентификатор документа. Защиты от повторной печати чека уже не будет. | `"9c5231d8-8075-4619-967e-8637e2a7b22d"`|
| TimeZone              | int                 | –        | Часовая зона:  <br>0 — Авто;  <br>1 — 1 часовая зона (МСК-1 / UTC+2);  <br>2 — 2 часовая зона (МСК / UTC+3);  <br>3 — 3 часовая зона (МСК+1 / UTC+4);  <br>4 — 4 часовая зона (МСК+2 / UTC+5);  <br>5 — 5 часовая зона (МСК+3 / UTC+6);  <br>6 — 6 часовая зона (МСК+4 / UTC+7);  <br>7 — 7 часовая зона (МСК+5 / UTC+8);  <br>8 — 8 часовая зона (МСК+6 / UTC+9);  <br>9 — 9 часовая зона (МСК+7 / UTC+10);  <br>10 — 10 часовая зона (МСК+8 / UTC+11);  <br>11 — 11 часовая зона (МСК+9 / UTC+12).<br><br>Если поле не указано, используется значение из поля «Часовая зона» в настройках ККТ (вкладка «Автозамена»). <br><br>Если в настройках ККТ для этого поля включена опция «Заменять принудительно», то значение из запроса заменяется на значение из настроек ККТ. | 9 |
| PaymentType           | int                 | +        | Тип чека:  <br>Текст = 0;  <br>Продажа = 1;  <br>Возврат = 2;  <br>Покупка = 3;  <br>ВозвратПокупки = 4;  <br>ЧекКоррекцииПрихода = 5;  <br>ЧекКоррекцииВозвратаПрихода = 6;  <br>ЧекКоррекцииРасхода = 7;  <br>ЧекКоррекцииВозвратаРасхода = 8.<br><br>Если поле не указано, по умолчанию используется `0`. <br><br>Типы 5–8 (коррекция) не поддерживаются в данном запросе – для них используйте отдельные эндпоинты `/correction120` (ФФД 1.2) или `/correction105` (ФФД 1.05). | 2 |
| Cashier               | [Cashier](#cashier)          | –        | Сведения о кассире (продавце). <br><br>Если тип данных **Cashier** отсутствует или все его поля пустые, данные о кассире не передаются в чек. | {"Name": "Иванов А.И.", "Vatin": "7722345678"} |
| CorrectionData        | [CorrectionData](#correctiondata) | -        | Данные коррекции. <br><br>Не поддерживается в данном запросе. Используйте отдельные эндпоинты `/correction120` (ФФД 1.2) или `/correction105` (ФФД 1.05).| {"CorrectionData":{"Type":0,"Description":"Основание коррекции","Date":"2026-03-13T00:00:00","Number":"0"}}|    
| Customer              | [Customer](#customer)          | –        | Сведения о покупателе. <br><br>Если тип данных **Customer** отсутствует или все его поля пустые, данные о покупателе не передаются в чек. | {"Info": "ООО 'Рога и Копыта'", "Vatin": "7722345678"} |
| TaxVariant            | int                 | +        | Система налогообложения (СНО):  <br>0 — ОСН,  <br>1 — УСН,  <br>2 — УСНД_Р,  <br>3 — ЕНВД,  <br>4 — ЕСН,  <br>5 — ПСН.<br><br>Если поле не указано, по умолчанию используется `0` (ОСН). <br><br>Если в настройках ККТ на вкладке «Автозамена» для поля «Система налогообложения» включена опция «Заменять принудительно», то значение из запроса заменяется на значение из настроек ККТ. | 1 |
| AgentSign             | int                 | –        | Признак агента:  <br>0 — Банковский платежный агент;  <br>1 — Банковский платежный субагент;  <br>2 — Платежный агент;  <br>3 — Платежный субагент;  <br>4 — Поверенный;  <br>5 — Комиссионер;  <br>6 — Агент.<br><br>Если поле не указано или пустое, признак агента не используется.| 3 |
| Electronically        | bool                | –        | Признак электронного чека (без печати на бумаге). <br><br>Если поле не указано, считается `false` (чек печатается на бумаге). <br><br>Контакт покупателя определяется из **Customer.Email** или **Customer.Phone**.<br><br>Если `Electronically = true`, но контакт не найден (ни в запросе, ни в настройках ККТ/сервера), сервер автоматически сбрасывает признак в `false`, и чек печатается на бумаге. <br><br>**Настройка «Отключение печати бумажных чеков»** (вкладка «Электронный чек» у ККТ или глобально на странице "Общие настройки", в категории "Электронный чек"): если включена — любой чек принудительно переводится в электронный (`Electronically = true`), независимо от значения в запросе.  <br>  <br>**Настройка «Печатать бумажный чек при наличии контакта покупателя»** (вкладка «Электронный чек» у ККТ или глобально на странице "Общие настройки", в категории "Электронный чек"): если включена и контакт покупателя заполнен — электронный чек из запроса (`Electronically = true`) **переводится в бумажный** (`false`).<br><br>По умолчанию обе настройки выключены. Глобальная настройка имеет приоритет над настройкой конкретной ККТ. | false |
| SenderEmail           | string              | –        | Адрес электронной почты отправителя чека. <br><br>Если поле не указано или пустое, адрес отправителя не используется. | "ivanov@mail.ru" |
| SaleAddress           | string              | –        | Адрес проведения расчётов. <br><br>Если поле не указано, используется адрес из настроек ККТ. <br><br>Если в настройках ККТ, во вкладке "Автоматические действия" выставлен флаг "Заменять принудительно", значение из запроса всегда заменяется адресом из вкладки «Автозамена».  | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation          | string              | –        | Место проведения расчётов. <br><br>Если поле не указано, используется значение из настроек ККТ. <br><br>Если в настройках ККТ, во вкладке "Автозамена" задано "Место расчётов" и в чеке поле пустое — используется значение из вкладки "Автозамена".  <br>  <br>Если в настройках ККТ, во вкладке "Автозамена" выставлен флаг "Заменять принудительно", значение из запроса заменяется значением из настроек ККТ.<br><br>Если также включена опция «Признак расчёта в интернете», драйвер позволяет вместо адреса указать ссылку на сайт в этом поле (тег 1187). | "Офис" |
| AgentData             | [AgentData](#agentdata) | –        | Данные агента. <br><br>Если тип данных не указан, данные агента не передаются в чек. <br><br>Если *AgentData** описан непосредственно в теле чека описан, то для каждой позиции, у которой отсутствуют собственные данные агента, используются значения из этого поля. <br><br>Обязательное связанное поле – **SignSubjectCalculationAgent** (признак агента у позиции или в заголовке). <br><br>Для кодов агента 0, 1, 2, 3 (банковские и платежные агенты) обязательны поля: **PayingAgentOperation**, **PayingAgentPhone**, **ReceivePaymentsOperatorPhone**.<br><br>Для кодов 0, 1 (банковские агенты) дополнительно обязательны все четыре поля группы **MoneyTransferOperator**: **MoneyTransferOperatorPhone**, **MoneyTransferOperatorName**, **MoneyTransferOperatorAddress**, **MoneyTransferOperatorVatin** – при заполнении хотя бы одного из них требуются все четыре, иначе чек будет отклонён с ошибкой. | {"PayingAgentOperation":"Прием платежей", "PayingAgentPhone": ["+79021654832"], "ReceivePaymentsOperatorPhone": ["+790216748367"], "MoneyTransferOperatorPhone": ["+790216702167"], "MoneyTransferOperatorName": "Иванов И.И.", "MoneyTransferOperatorAddress": "г.Улан-Удэ, ул.Виноградная, д11А, офис 25", "MoneyTransferOperatorVatin": "7722345678"} |
| Vendor                | [VendorData](#vendordata) | –        | Данные поставщика (используются при продаже через агента). <br><br>Если тип данных не указан, данные поставщика не передаются. <br><br>Если **Vendor** указан непосредственно в теле чека, то для позиций без собственных данных поставщика применяются значения из этого поля.<br><br>Обязательно указание **SignSubjectCalculationAgent** в позиции – при отсутствии признака агента чек будет отклонён с ошибкой «Признак агента отсутствует» | {"Phones": ["+79031234567", "+79169876543"], "Name": "ИП 'Ромашка'", "Vatin": "5262107639"} |
| ElectronicPaymentInfo | [ApiElectronicPayment](#apielectronicpayment)[] | – | Сведения об оплате безналичными. <br><br>Если массив не указан, информация о безналичной оплате не передаётся. <br><br>Используется только при оплате безналичными средствами (**Payments.ElectronicPayment** > 0). <br><br>Сумма всех элементов **Amount** должна **точно** совпадать с **Payments.ElectronicPayment**, иначе запрос завершится ошибкой «Несовпадение суммы безналичной оплаты и суммы платежей по эквайрингу». | [{"Amount": 50, "PaymentMethod": 3, "Identifiers": "RRN=123456789012", "AdditionalInformation": "Терминал №1, карта *1234"}] |
| OperationalAttribute  | [OperationalAttribute](#operationalattribute)  | –        | Операционный реквизит чека (тег 1270). <br><br>Если тип данных не указан, реквизит не передаётся. | {"DateTime": "03.07.2026", "OperationId": "24f4bffe-98ef-4627-846c-b1f74c5a495b", "OperationData": "Оплата по договору №784/2026"} |
| OperationOnline       | bool                | –        | Признак применения ККТ при расчёте в безналичной форме в сети «Интернет». <br><br>Если поле не указано, считается `false`. <br><br>Если в настройках ККТ на вкладке «Автозамена» включена опция «Признак расчёта в интернете», то значение поля принудительно устанавливается в `true` для всех чеков. <br><br>При включении этой опции драйвер также позволяет указать ссылку на сайт в поле **SaleLocation** (тег 1187) – подробнее см. описание поля **SaleLocation**. | false |
| AdditionalAttribute   | string              | –        | Дополнительный реквизит чека (тег 1192). <br><br>Если поле не указано или пустое, реквизит не передаётся. <br><br>Рекомендуется указывать здесь ФП корректируемого чека при оформлении возврата или коррекции.  | `"6702704322"` |
| IndustryAttribute     | [IndustryAttribute](#industryattribute) | –        | Отраслевой реквизит чека (тег 1261). | {"IdentifierFoiv": "3", "DocumentDate": "01.07.2026", "DocumentNumber": "6", "AttributeValue": "UUID=8f3a9d1c-7e2b-4a5f-9c8d-1e2f3a4b5c6d&Time=2088157392047"} |
| UserAttribute         | [UserAttribute](#userattribute) | –        | Дополнительный реквизит пользователя (тег 1086). <br><br>Если тип данных не указан, реквизит не передаётся. <br><br>Оба поля – **Name** и **Value** – должны быть заполнены, иначе реквизит не будет передан. | {"Name": "НомерЗаказа", "Value": "ORD-2026-0042"} |
| Payments              | [ApiPayments](#apipayments) | +        | Способы оплаты. Сам метод оплаты является опциональным и выбирается пользователем в зависимости от конкретного запроса. <br><br>Если тип данных не указан, все суммы оплат считаются равными 0 – чек не закроется (сумма оплат должна быть не меньше суммы позиций). <br><br>Если тип данных передан, но отдельные поля (**Cash**, **ElectronicPayment**, **AdvancePayment**, **Credit**, **CashProvision**) отсутствуют, они считаются равными 0. <br><br>Допускается комбинированная оплата: наличными (**Cash**), безналичными (**ElectronicPayment**), авансом (**AdvancePayment**), постоплатой (**Credit**) или встречным предоставлением (**CashProvision**). | {"Cash": 50, "ElectronicPayment": 0, "AdvancePayment": 0, "Credit": 0, "CashProvision": 0} |
| Positions             | [Position](#position--checktemplatedocumentparameters)[] | +        | Массив позиций чека.<br><br> Каждый элемент может содержать фискальную строку (**FiscalString**), текстовую строку (**TextString**), штрихкод (**Barcode**), картинку (**Picture**) или разделительную линию (**SeparatorLine**). <br><br>Позиции, содержащие нефискальные строки не влияют на итоговую сумму.                                     

**Тело ответа**

**Result (FiscalParams)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| datetime       | datetime | - | Время операции | "2026-05-25T12:01:47.4455976+08:00"    |
| deviceName     | string   | + | Название устройства | "Atol"  |
| docId          | string   | + | Идентификатор документа чека. Передайте в GET-запрос, чтобы получить результат операции | "4bdcaa12-15a3-470c-b07d-3a498242193a" |
| fnsUrl         | string   | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru"  |
| fnNumber       | string   | - | Номер фискального накопителя  | "0123123123123"  |
| rnNumber       | string   | - | Регистрационный номер ККТ | "00031415926" |
| fiscalDatetime | string   | - | Дата и время документа по часам ФН  | "20260525120147" |
| fiscalSign | string | - | Фискальный признак документа| "5847203916" |
| shiftNumber    | int      | + | Номер смены   | 1  |
| fiscalNumber   | int      | - | Номер фискального документа | 3  |

### `PrintCheckAsync()`
`POST check/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно поставить фискальный чек в очередь печати.

**Тело запроса**
Смотрите документацию [POST check](#printcheck)

**Тело ответа**

**Result**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа чека. Передайте в GET-запрос, чтобы получить результат операции | "31f9d8a2-6424-4085-807c-a349d51884b1" |

### `GetCheck()`
`GET check`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает результат операции по идентификатору документа (docId), полученному в POST-запросе.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа чека | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (Check)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| CheckItems | [CheckItem](#checkitem)[] | - | Позиции чека | "CheckItems": [ <br>  {<br>"Name": "Бутылка с водой 1л.", <br>"Quantity": 1, <br>"Price": 50, <br>"Department": 0, <br>"Sum": 50, <br>"IsFiscal": true, <br>"TaxValue": 20, <br>"PaymentMode": 3, <br>"ItemType": 10, <br>"ExciseAmount": 0, <br>"MeasureOfQuantity": 20 <br>} <br>], |
| TrustedInFn | bool | - | Подтвержден в ФН | false |
| IsFiscal | bool | - | Фискальный режим | true |
| Change | decimal | - | Сдача | 0.0 |
| Sum | decimal | - | Сумма с учетом скидки | 50.0 |
| OperationOnline | bool | - | Признак применения ККТ при осуществлении расчета в безналичном порядке в сети "Интернет" | true |
| ClientContact | string | - | Номер телефона или электронная почта клиента | "kuznicov@mail.ru" |
| CustomerDetail | [CustomerDetail](#customerdetail) | - | Cведения о покупателе (клиенте) | "CustomerDetail": {<br>"Info": "ООО 'Рога и Копыта'", <br>"Vatin": "500100732259", <br>"Email": "kuznicov@mail.ru" <br>}, |
| QrData | [QrCheckData](#qrcheckdata) | - | Данные для отображения QR кода чека | "QrData": { <br>"Date": "2026-05-24T12:52:00+08:00", <br>"Amount": 50, <br>"Fn": "9999078902010507", <br>"Fd": 370, <br>"Fp": "2928907410",<br>"N": 1 <br>}, |
| Payments | [Payments](#payments) | - | Оплаты | "Payments": { <br>"Cash": 50, <br>"Electronic": 0, <br>"PrePaid": 0, <br>"Credit": 0, <br>"Barter": 0 <br>}, |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": { <br>"OrganizationInfo": "ООО 'Ромашка'", <br>"SerialNumber": "00106305393630", <br>"Vatin": "7722345678", <br>"Cashier": "Иванов А.И.", <br>"RnNumber": "0000000002005725", <br>"Fn": "9999078902010507", <br>"FnsUrl": "nalog.ru", <br>"ShiftNumber": 41, <br>"DocNumber": 370, <br>"FiscalSign": "2928907410", <br>"OfdOrganizationName": "Тестовый ОФД", <br>"OfdVatin": "7709364346" <br>}, |
| Electronically | bool | - | Регистрация чека без печати на ленте | false |
| TaxType | int | - | Код налогообложения (СНО): <br>0 — ОСН, <br>1 — УСН, <br>2 — УСНД_Р, <br>3 — ЕНВД, <br>4 — ЕСН, <br>5 — ПСН. | 1 |
| IsReplaceTax | bool | - | Замена НДС | true |
| TimeZone | int | - | Часовая зона: <br>0 — Авто; <br>1 — 1 часовая зона (МСК-1 / UTC+2); <br>2 — 2 часовая зона (МСК / UTC+3); <br>3 — 3 часовая зона (МСК+1 / UTC+4); <br>4 — 4 часовая зона (МСК+2 / UTC+5); <br>5 — 5 часовая зона (МСК+3 / UTC+6); <br>6 — 6 часовая зона (МСК+4 / UTC+7); <br>7 — 7 часовая зона (МСК+5 / UTC+8); <br>8 — 8 часовая зона (МСК+6 / UTC+9); <br>9 — 9 часовая зона (МСК+7 / UTC+10); <br>10 — 10 часовая зона (МСК+8 / UTC+11); <br>11 — 11 часовая зона (МСК+9 / UTC+12) | 2 |
| ShiftNumber | int | + | Номер сессии. <br>Используется для [GET check/list](#getchecklist) для получения списка чеков | 3 |
| DocNumber | int | + | Номер фискального документа. <br>Используется для [GET check/fiscalSign](#getfiscalsign) для получения фискального признака | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | "1209,Номер версии ФФД:4\\r\\n1041,Номер ФН:999907890..." |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор документа чека. Передайте в GET-запрос, чтобы получить результат операции | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": { <br>"FfdVersion": "1.2", <br>"FnFfdVersion": "1.2", <br>"TimeZone": 7, <br>"KktLicenses": [], <br>"IsFiscal": true, <br>"LineLength": 64, <br>"LineLengthPixels": 576, <br>"DeviceClass": 4, <br>"Model": "АТОЛ FPrint-22ПТК", <br>"SerialNumber": "00106305393630", <br>"FirmwareVersion": "5.15.102", <br>"ConfigurationVersion": "5.17.0" <br>} |

### `GetCheckList()`
`GET check/list`

**Заполняет:** `Checks` (те же списки: `Slips`, `CashIns`, `Corrections105`, `Corrections120`).

Получение списка чеков за смену

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |
| ShiftsFrom | DateTime | + | Начало даты отбора (формат: гггг-мм-дд). | 2026-06-01 |
| ShiftsTo | DateTime | + | Конец даты отбора (формат: гггг-мм-дд). | 2026-06-30 |
| ShiftNumber | int | + | Номер смены | 4 |

**Тело ответа**

**Result (Check[])**
Смотрите документацию [GET check](#getcheck)

### `PrintCheckCopy()`
`POST check/copy`

**Заполняет:** только статус.

Печать копии чека по `DocumentId` (Guid документа или ФП чека).

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocId | string | + | Идентификатор документа, для которого необходимо выполнить повторную печать. <br><br>По указанному идентификатору сервер выполняет поиск чека во внутренней базе данных. Если чек найден, на ККМ отправляется задание на печать его копии. <br>Если документ с указанным идентификатором не найден, запрос завершается ошибкой. |
| DeviceName | string | + | Имя устройства. Если имя не указано, будет использована касса, указанная в чеке. <br>Если эта касса недоступна, будет выбрана первая свободная. | "Atol" |

**Тело ответа**

Только статус (`Code`, `Description`, `Success`), без `Result`.

### `PrintLastCheckCopy()`
`POST check/copy/last`

**Заполняет:** только статус.

Печать копии последнего чека на кассе `DeviceName`.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| device | string | + | Имя устройства | Atol |

**Тело ответа**

Только статус (`Code`, `Description`, `Success`), без `Result`.

### `PrintCheckCopyFn()`
`POST check/copy/fn`

**Заполняет:** только статус.

Печать копии чека из ФН по `DocumentId` (Guid документа или ФП чека). Выполняется только на ККТ исходного чека.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocId | string | + | Guid документа или фискальный признак (ФП) чека | "855fa6cb-5e05-4983-8e39-de5187ac7a21" |
| DeviceName | string | – | Имя устройства | "Atol" |

**Тело ответа**

Только статус (`Code`, `Description`, `Success`), без `Result`.

### `GetFiscalSign()`
`GET check/fiscalSign`

**Заполняет:** `FiscalSign`.

Получение фискального признака (ФП) по номеру фискального документа (ФД)

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |
| CheckNumber | string | + | Номер фискального документа. | 8 |

**Тело ответа**

**Result**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | - | Фискальный признак | "1807094707" |

### `GetTaskStatus()`
`GET task/status`

**Заполняет:** `TaskStatus…`, `DocumentId`, `FiscalSign`, `CheckNumber`, `ShiftNumber`.

Возвращает статус выполнения задания по идентификатору документа (docId), полученному в ответе на любой POST-запрос.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа (docId) из ответа POST-запроса. | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа:**

**Result (ResponseGetStatus)**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| DocId | string | + | Идентификатор документа | "855fa6cb-5e05-4983-8e39-de5187ac7a21" |
| Date | datetime | - | Дата и время постановки задания в обработку | "2026-03-18T16:23:59" |
| SentToPrint | int | - | Статус отправки: <br>0 — Задача новая, в очереди, <br>1 — Задача отправлена на выполнение, <br>2 — Задача удачно обработана, <br>-1 — Задача вернулась из обработки с ошибкой, | -1 |
| NumberInQueue | int | - | Позиция задания в очереди на момент запроса. <br>-1 — задание уже покинуло очередь (обработано или завершилось ошибкой). | -1 |
| QueueSize | int | - | Размер очереди | 0 |
| PoolId | string | - | Идентификатор пула, в рамках которого обрабатывалось задание. Если устройство не входит в пул — не заполняется. | "pool" |
| ShiftNumber | int | + | Номер смены | 306 |
| DocNumber | int | + | Номер чека | 1373 |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 1 |
| FiscalSign | string | - | Фискальный признак. Фискальный признак документа. Заполняется только для фискальных документов | "1173363965" |
| ResultCode | int | - | Код результата обработки задания | 0 |
| ResultDescription | string | - | Описание результата обработки задания | "OK" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | Заголовок фискального документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00106305393630",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000002005725",<br>"Fn": "9999078902010507",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 39,<br>"DocNumber": 343,<br>"FiscalSign": "1352614355",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>}  |

### `GetPrintForm()`
`GET task/form`

**Заполняет:** `PrintForm`.

Возвращает печатную форму документа по его идентификатору (docId), полученному в ответе на любой POST-запрос.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа (docId) из ответа POST-запроса. | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (PrintLine[])**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | int | - | Тип строки.  <br>0 — фискальная строка;  <br>1 — текстовая строка;  <br>2 — штрихкод;  <br>3 — изображение;  <br>4 — разделительная линия | 1 |
| Width | int | - | Ширина | 0 |
| Scale | int | - | Масштаб | 100 |
| Line | string | - | Текст строки (левая часть) | "Кассовый чек" |
| LineRight | string | - | Текст строки (правая часть) | "ЧЕК 2" |
| Alignment | int | - | Выравнивание.  <br>0 — выравнивание по левому краю;  <br>1 — Выравнивание по центру;  <br>2 — Выравнивание по правому краю;  <br>3 — По ширине | 1 |
| Font | int | - |Шрифт. <br>0 — Шрифт для обычных строк;  <br>1 — Жирный шрифт;  <br>2 — Мелкий шрифт;  <br>3 — Средний шрифт;  <br>4 — Крупный шрифт ;  <br>5 — Стиль заголовка первого уровня (H1);  <br>6 — Стиль заголовка второго уровня (H2);  <br>7 — Стиль заголовка третьего уровня (H3);  <br>8 — Стиль заголовка четвёртого уровня (H4);  <br>9 — Стиль заголовка пятого уровня (H5) | 0 |
| IsFontSpecified | bool | - | Признак, что шрифт задан явно во входящих данных или при создании строки| true |
| Wrap | bool | - | Признак переноса строк.  <br>false - строка обрезается;  <br>true - строка переносится | true |
| Barcode | [Barcode](#barcode) | - | Штрихкод | "Barcode": {<br>"Type": "QR",<br>"Value": "t=20260522T1506&s=50.00&fn=9999078902010507&i=343&fp=1352614355&n=1",<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAH0AAAB9CAIAAAAA4vtyAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAgrSURBVHhe7ZHBjuRKDgPn/39691AAkVJksimXpz0PcBypoJTl+v...",<br>"PrintText": 1,<br>"Height": 100,<br>"BarWidth": 100<br>} |
| SeparatorLine | [SeparatorLine](#separatorline) | - | Разделительная линиия | "SeparatorLine": {<br>"LineStyle": 0<br>} |
| IsCreateFromTemplate | bool | - | Признак создания строки из печатного шаблона. <br>true - создано из печатного шаблона;  <br>false — не создан из печатного шаблона | false |
| BarcodeLines | string[] | - | Массив строк, выводимые спрва или слева от штрихакодв | "ЗН ККТ: 0020260207",  <br>"РН ККТ: 00031415926",  <br>"ИНН 7722345678",  <br>"ФН: 0123123123123",  <br>"ФД: 343",  <br>"ФП: 1352614355",  <br>"ПРИХОД",  <br>"22.05.26 15:06",  <br>"Сайт ФНС:  <br>[nalog.ru"](http://www.nalog.gov.ru) |

---

## Чеки коррекции

### `PrintCheckCorrection120()`
`POST correction120`

**Заполняет:** `DocumentId`, `FiscalSign`, `CheckNumber`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Печать чека коррекции для ФФД 1.2

**Тело запроса**
Смотрите документацию [POST check](#printcheck)

**Тело ответа**

**Result (FiscalParams)**
Смотрите документацию [POST check](#printcheck)

### `PrintCheckCorrection120Async()`
`POST correction120/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно печатает чек коррекции для ФФД 1.2

**Тело запроса**
Смотрите документацию [POST correction120](#printcheckcorrection120)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа чека коррекции ФФД 1.2. Передайте в [GET correction120](#getcheckcorrection120), чтобы получить чек коррекции ФФД 1.2. | "cbac9c78-c4f5-4f03-8be9-696136ebc9ab" |

### `PrintCheckCorrection105()`
`POST correction105`

**Заполняет:** `DocumentId`, `FiscalSign`, `CheckNumber`, `ShiftNumber`, `FnNumber`, `RnNumber`, `FnsUrl`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Печать чека коррекции для ФФД 1.05

**Тело запроса**

| **Имя поля**          | **Тип**             | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string              | +        | Имя устройства. | "Atol" |
| DocId | string | – | Идентификатор документа. Если указан, то сервер сначала произведет поиск чека во внутренней базе данных. В случае, если чек будет найден, считается, что задание пошло на повторную печать.  <br>  <br>Если предыдущая попытка печати была завершена успешно, то в ответ на запрос вернется результат предыдущей печати.  <br>Повторного исполнения задания на ККМ не будет. Это защита от ошибочной повторной регистрации чеков. Такие случае могут возникать в случае обрыва сетевой связи с клиентом во время обработки запроса на печать.  <br>  <br>Если же предыдущая попытка печати была неудачной, то задание будет перезаписано и проведена печать чека на ККМ.  <br>  <br>Если идентификатор документа не указан, то будет создан новый идентификатор документа. Защиты от повторной печати чека уже не будет.| "9c5231d8-8075-4619-967e-8637e2a7b22d"  |
| PaymentType           | int                 | +        | Тип чека:  <br>Текст = 0;  <br>Продажа = 1;  <br>Возврат = 2;  <br>Покупка = 3;  <br>ВозвратПокупки = 4;  <br>ЧекКоррекцииПрихода = 5;  <br>ЧекКоррекцииВозвратаПрихода = 6;  <br>ЧекКоррекцииРасхода = 7;  <br>ЧекКоррекцииВозвратаРасхода = 8.<br><br>Если поле не указано, по умолчанию используется `0`. <br><br>Типы 5–8 (коррекция) не поддерживаются в данном запросе – для них используйте отдельные эндпоинты `/correction120` (ФФД 1.2) или `/correction105` (ФФД 1.05). | 2 |
| Cashier | [Cashier](#cashier) | +        | Сведения о кассире (продавце). <br><br>Если тип данных **Cashier** отсутствует или все его поля пустые, данные о кассире не передаются в чек и операция завершится с ошибкой. | `{"Name": "Иванов А.И.", "Vatin": "7722345678"} |
| CorrectionData        | [CorrectionData](#correctiondata)         | +        | Данные коррекции. | `{"CorrectionData":{"Type":0,"Description":"Основание коррекции","Date":"2026-03-13T00:00:00","Number":"0"}}` |                           
| Customer              | [Customer](#customer)          | –        | Сведения о покупателе. <br><br>Если тип данных **Customer** отсутствует или все его поля пустые, данные о покупателе не передаются в чек. | `{"Info": "ООО 'Рога и Копыта'", "Vatin": "7722345678"}` |
| TaxVariant            | int                 | +        | Система налогообложения (СНО):  <br>0 — ОСН,  <br>1 — УСН,  <br>2 — УСНД_Р,  <br>3 — ЕНВД,  <br>4 — ЕСН,  <br>5 — ПСН.<br><br>Если поле не указано, по умолчанию используется `0` (ОСН). <br><br>Если в настройках ККТ на вкладке «Автозамена» для поля «Система налогообложения» включена опция «Заменять принудительно», то значение из запроса заменяется на значение из настроек ККТ. | `1` |             
| AdditionalAttribute   | string              | –        | Дополнительный реквизит чека (тег 1192). <br><br>Если поле не указано или пустое, реквизит не передаётся. <br><br>Рекомендуется указывать здесь ФП корректируемого чека при оформлении возврата или коррекции. | `"6702704322"` |
| SumTax0        | decimal     | -        | Сумма расчёта по ставке НДС 0%. Если не указано — 0. | 0           |
| SumTax5        | decimal     | -        | Сумма НДС чека по ставке 5%. Если не указано — 0. | 0           |
| SumTax7        | decimal     | -        | Сумма НДС чека по ставке 7%. Если не указано — 0. | 0           |
| SumTax10       | decimal     | -        | Сумма НДС чека по ставке 10%. Если не указано — 0. | 0           |
| SumTax18       | decimal     | -        | Сумма НДС чека по ставке 18%. Если не указано — 0.  | 0           |
| SumTax20       | decimal     | -        | Сумма НДС чека по ставке 20%. Если не указано — 0. | 100         |
| SumTax22       | decimal     | -        | Сумма НДС чека по ставке 22%. Если не указано — 0. | 0           |
| SumTaxNone     | decimal     | -        | Сумма расчёта без НДС. Если не указано — 0. | 0           |
| SumTax105      | decimal     | -        | Сумма НДС чека по расчётной ставке 5/105. Если не указано — 0. | 0           |
| SumTax107      | decimal     | -        | Сумма НДС чека по расчётной ставке 7/107. Если не указано — 0. | 0           |
| SumTax110      | decimal     | -        | Сумма НДС чека по расчётной ставке 10/110. Если не указано — 0. | 0           |
| SumTax118      | decimal     | -        | Сумма НДС чека по расчётной ставке 18/118. Если не указано — 0.  | 0 |
| SumTax120      | decimal     | -        | Сумма НДС чека по расчётной ставке 20/120. Если не указано — 0. |
| SumTax122      | decimal     | - | Сумма НДС чека по расчётной ставке 22/122. Если не указано — 0. | 0 |
| Payments              | [ApiPayments](#apipayments)  | + | Способы оплаты. Сам метод оплаты является опциональным и выбирается пользователем в зависимости от конкретного запроса. <br><br>Если тип данных не указан, все суммы оплат считаются равными 0 – чек не закроется (сумма оплат должна быть не меньше суммы позиций). <br><br>Если тип данных передан, но отдельные поля (**Cash**, **ElectronicPayment**, **AdvancePayment**, **Credit**, **CashProvision**) отсутствуют, они считаются равными 0. <br><br>Допускается комбинированная оплата: наличными (**Cash**), безналичными (**ElectronicPayment**), авансом (**AdvancePayment**), постоплатой (**Credit**) или встречным предоставлением (**CashProvision**). | `{"Cash": 50, "ElectronicPayment": 0, "AdvancePayment": 0, "Credit": 0, "CashProvision": 0}` |

**Тело ответа**

**Result (FiscalParams)**
Смотрите документацию [POST check](#printcheck)

### `PrintCheckCorrection105Async()`
`POST correction105/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно ставит печать чека коррекции для ФФД 1.0.5

**Тело запроса**
Смотрите документацию в [POST correction105](#printcheckcorrection105)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- |
| Result | string | + | Идентификатор документа чека коррекции ФФД 1.0.5. Передайте в [GET correction105](#getcheckcorrection105), чтобы получить чек коррекции. | "cbac9c78-c4f5-4f03-8be9-696136ebc9ab" |

### `GetCheckCorrection120()`
`GET correction120`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает чек коррекции ФФД 1.2 по идентификатору документа (docId), полученному в [POST correction120](#printcheckcorrection120).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа чека коррекции ФФД 1.2 | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (CheckCorrection120)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| CheckItems | [CheckItem](#checkitem)[] | - | Позиции чека | "CheckItems": [], |
| Electronically | bool | - | Регистрация чека без печати на ленте | false |
| Payments | [ApiPayments](#apipayments) | - | Оплаты чека | "Payments": {<br>"Cash": 100,<br>"Electronic": 0,<br>"PrePaid": 0,<br>"Credit": 0,<br>"Barter": 0<br>} |
| TaxType | int | Код налогообложения: <br>0 — ОСН, <br>1 — УСН, <br>2 — УСНД_Р, <br>3 — ЕНВД, <br>4 — ЕСН, <br>5 — ПСН | 1 |
| TaxSum18 | decimal | - | Сумма НДС 18 | 0.0 |
| TaxSum10 | decimal | - | Сумма НДС 10 | 0.0 |
| TaxSum7 | decimal | - | Сумма НДС 7 | 0.0 |
| TaxSum5 | decimal | - | Сумма НДС 5 | 0.0 |
| TaxSum105 | decimal | - | Сумма НДС 5/105 | 0.0 |
| TaxSum107 | decimal | - | Сумма НДС 7/107 | 0.0 |
| TaxSum0 | decimal | - | Сумма НДС 0 | 0.0 |
| TaxSum110 | decimal | - | Сумма НДС 110 | 0.0 |
| TaxSumNone | decimal | - | Сумма БЕЗ НДС | 0.0 |
| TaxSum118 | decimal | - | Сумма НДС 118 | 0.0 |
| TaxSum120 | decimal | - | Сумма НДС 120 | 0.0 |
| TaxSum20 | decimal | - | Сумма НДС 20 | 100 |
| TaxSum122 | decimal | - | Сумма НДС 122 | 0.0 |
| TaxSum22 | decimal | - | Сумма НДС 22 | 0.0 |
| ShiftNumber | int | - | Номер сессии | 3 |
| DocNumber | int | - | Номер фискального документа | 5 |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00109325182732",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000001002520",<br>"Fn": "9999078902012910",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"CheckNumber": 20,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | "" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| TaxType | int | - | Код налогообложения (СНО): <br>0 — ОСН, <br>1 — УСН, <br>2 — УСНД_Р, <br>3 — ЕНВД, <br>4 — ЕСН, <br>5 — ПСН. | 1 |
| CustomerDetail | [CustomerDetail](#customerdetail) | - | Cведения о покупателе (клиенте) | "CustomerDetail": {<br>"Info": "ООО 'Рога и Копыта'", <br>"Vatin": "500100732259", <br>"Email": "kuznicov@mail.ru" <br>}, |
| QrData | [QrCheckData](#qrcheckdata) | - | Данные для отображения QR кода чека | "QrData": { <br>"Date": "2026-05-24T12:52:00+08:00", <br>"Amount": 50, <br>"Fn": "9999078902010507", <br>"Fd": 370, <br>"Fp": "2928907410",<br>"N": 1 <br>}, |
| DocId | string | + | Идентификатор документа чека коррекции ФФД 1.2 | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.0.5",<br>"FnFfdVersion": "1.0.5",<br>"IsFiscal": true,<br>"LineLength": 42,<br>"DeviceClass": 4,<br>"Model": "АТОЛ 1Ф",<br>"SerialNumber": "00109325182732",<br>"FirmwareVersion": "5.8.1",<br>"ConfigurationVersion": "5.8.17"<br>} |

### `GetCheckCorrection120List()`
`GET correction120/list`

**Заполняет:** `Checks` (те же списки: `Slips`, `CashIns`, `Corrections105`, `Corrections120`).

Получение списка чеков коррекции ФФД 1.2

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result**

**Тип: (CheckCorrection120[])**
Смотрите документацию [GET correction120](#getcheckcorrection120)

### `GetCheckCorrection105()`
`GET correction105`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает чек коррекции ФФД 1.0.5 по идентификатору документа (docId), полученному в [POST correction105](#printcheckcorrection105).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа чека коррекции ФФД 1.0.5 | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (CheckCorrection105)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| CheckItems | [CheckItem](#checkitem)[] | - | Позиции чека | "CheckItems": [], |
| Electronically | bool | - | Регистрация чека без печати на ленте | false |
| Payments | [ApiPayments](#apipayments) | - | Оплаты чека | "Payments": {<br>"Cash": 100,<br>"Electronic": 0,<br>"PrePaid": 0,<br>"Credit": 0,<br>"Barter": 0<br>} |
| CorrectionData | [CorrectionData](#correctiondata) | - | Данные коррекции | "CorrectionData": {<br>"Type": 0,<br>"Description": "",<br>"Date": "2026-03-12T00:00:00",<br>"Number": "10"<br>} |
| TaxType | int | - | Код налогообложения: <br>0 — ОСН, <br>1 — УСН, <br>2 — УСНД_Р, <br>3 — ЕНВД, <br>4 — ЕСН, <br>5 — ПСН | 1 |
| TaxSum18 | decimal | - | Сумма НДС 18 | 0.0 |
| TaxSum10 | decimal | - | Сумма НДС 10 | 0.0 |
| TaxSum7 | decimal | - | Сумма НДС 7 | 0.0 |
| TaxSum5 | decimal | - | Сумма НДС 5 | 0.0 |
| TaxSum105 | decimal | - | Сумма НДС 5/105 | 0.0 |
| TaxSum107 | decimal | - | Сумма НДС 7/107 | 0.0 |
| TaxSum0 | decimal | - | Сумма НДС 0 | 0.0 |
| TaxSum110 | decimal | - | Сумма НДС 110 | 0.0 |
| TaxSumNone | decimal | - | Сумма БЕЗ НДС | 0.0 |
| TaxSum118 | decimal | - | Сумма НДС 118 | 0.0 |
| TaxSum120 | decimal | - | Сумма НДС 120 | 0.0 |
| TaxSum20 | decimal | - | Сумма НДС 20 | 0.0 |
| TaxSum122 | decimal | - | Сумма НДС 122 | 0.0 |
| TaxSum22 | decimal | - | Сумма НДС 22 | 0.0 |
| AdditionalAttribute      | string       | - | Дополнительный реквизит чека (БСО, тег 1192)| "6702704322" |
| ShiftNumber | int | + | Номер сессии | 3 |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00109325182732",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000001002520",<br>"Fn": "9999078902012910",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"CheckNumber": 20,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| DocNumber | int | + | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | "1209,Номер версии ФФД:4\\r\\n1041,Номер ФН:999907890..." |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + |Идентификатор документа чека коррекции ФФД 1.0.5 | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.0.5",<br>"FnFfdVersion": "1.0.5",<br>"IsFiscal": true,<br>"LineLength": 42,<br>"DeviceClass": 4,<br>"Model": "АТОЛ 1Ф",<br>"SerialNumber": "00109325182732",<br>"FirmwareVersion": "5.8.1",<br>"ConfigurationVersion": "5.8.17"<br>} |

### `GetCheckCorrection105List()`
`GET correction105/list`

**Заполняет:** `Checks` (те же списки: `Slips`, `CashIns`, `Corrections105`, `Corrections120`).

Получение списка чеков коррекции ФФД 1.05

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (CheckCorrection105[])**
Смотрите документация [GET correction105](#getcheckcorrection105)

---

## Денежный ящик

### `CashIn()`
`POST cashin`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Регистрация операции внесения наличных в денежный ящик.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| Sum | decimal | + | Сумма внесения. Если указано значение меньше или равное 0, запрос завершается ошибкой | 5 |
| Cashier       | [Cashier](#cashier)  | -        |  Сведения о кассире (продавце). <br><br>Если тип данных Cashier отсутствует или все его поля пустые, данные о кассире в документ не попадают. | "Cashier": {<br>"Name": "Иванов А.И.",<br>"Vatin": "7722345678"<br>} |

**Тело ответа**

**Result (CashIn)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Sum | decimal | - | Фактическая сумма внесения | 5.0 |
| CashSum | decimal | - | Сумма наличных в денежном ящике | 5.0 |
| ShiftNumber | int | - | Номер сессии | 41 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "5847203916" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00106305393630",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000002005725",<br>"Fn": "9999078902010507",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 41,<br>"DocNumber": 369,<br>"FiscalSign": "3411863455",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор операции внесения наличных. Передайте в [GET cashin](#getcashin), чтобы получить результат внесения наличных. | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |

### `CashInAsync()`
`POST cashin/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно поставить операцию внесения наличных в очередь

**Тело запроса**
Смотрите документацию [POST cashin](#cashin)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа операции внесения наличных. Данный идентфикатор можно использовать для [GET cashin](#getcashin), чтобы получить более подробную информацию. | "788eeda8-c2aa-4b51-8f39-db93d8e79b91" |

### `CashOut()`
`POST cashout`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Регистрация операции выемки наличных из денежного ящика.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Название устройства | "Atol" |
| Sum           | decimal     | +        | Сумма выемки. Если указано значение меньше или равное 0, запрос завершается ошибкой. <br><br>Если запрошенная сумма превышает фактический остаток наличных в денежном ящике, запрос завершается ошибкой "Недостаточно денег в денежном ящике". | 5  |
| Cashier       | [Cashier](#cashier)  | -        |  Сведения о кассире (продавце). <br><br>Если тип данных Cashier отсутствует или все его поля пустые, данные о кассире в документ не попадают. | "Cashier": {<br>"Name": "Иванов А.И.",<br>"Vatin": "7722345678"<br>} |

**Тело ответа**

**Result (CashOut)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Sum | decimal | - | Фактическая сумма внесения | 5.0 |
| CashSum | decimal | - | Сумма наличных в денежном ящике | 5.0 |
| ShiftNumber | int | - | Номер сессии | 41 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00106305393630",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000002005725",<br>"Fn": "9999078902010507",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 41,<br>"DocNumber": 369,<br>"FiscalSign": "5847203916",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор операции выемки наличных. Передайте в [GET cashout](#getcashout), чтобы получить результат выемки наличных. | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |

### `CashOutAsync()`
`POST cashout/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно регистрирует операцию выемки наличных в очередь

**Тело запроса**
Смотрите документацию [POST cashout](#cashout)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор документа операции выемки наличных. Данный идентфикатор можно использовать для [GET cashout](#getcashout), чтобы получить более подробную информацию. | "788eeda8-c2aa-4b51-8f39-db93d8e79b91" |

### `OpenCashdrawer()`
`POST cash/open`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Открытие денежного ящика

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| Cashier       | [Cashier](#cashier)  | - | Сведения о кассире (продавце). <br><br>Если тип данных Cashier отсутствует или все его поля пустые, данные о кассире в документ не попадают. | "Cashier": {<br>"Name": "Иванов А.И.",<br>"Vatin": "7722345678"<br>} |

**Тело ответа**

**Result (OpenDrawer)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DrawerNumber | int | - | Номер денежного ящика | 1 |
| ShiftNumber | int | - | Номер сессии | 41 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "5847203916" |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа| "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00106305393630",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000002005725",<br>"Fn": "9999078902010507",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 41,<br>"DocNumber": 369,<br>"FiscalSign": "3411863455",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 11 |
| DocId | string | + | Идентификатор документа открытия смены | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | - | string | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |

### `GetCash()`
`GET cash`

**Заполняет:** `CashBalance`.

Коннектор кладёт `Result.Sum` в свойство `CashBalance`.

Получение остатка наличных в денежном ящике

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (CashSum)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Sum | decimal | + | Остаток наличных в денежном ящике | 4683.81 |

### `GetCashIn()`
`GET cashin`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает результат операции внесения наличных по идентификатору операции (docId), полученному в [POST cashin](#cashin).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор операции внесения наличных | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (CashIn)**
Смотрите документацию [POST cashin](#cashin)

### `GetCashInList()`
`GET cashin/list`

**Заполняет:** `Checks` (те же списки: `Slips`, `CashIns`, `Corrections105`, `Corrections120`).

Получение списка операций внесения наличных по имени устройства.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (CashIn[])**
Смотрите документацию [GET cashin](#getcashin)

### `GetCashOut()`
`GET cashout`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает результат операции выемки наличных по идентификатору операции (docId), полученному в [POST cashout](#cashout).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор операции выемки наличных | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (CashOut)**
Смотрите документацию [POST cashout](#cashout)

---

## Нефискальные чеки

### `PrintSlip()`
`POST slip`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Печатать нефискального документа

**Тело запроса**

| **Имя поля** | **Тип**       | **Обяз** | **Назначение**      | **Пример**                                                                                                                                                                                                                                                                                                                   |
| ------------ | ------------- | -------- | ------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| DeviceName   | string        | +        | Название устройства | "Atol"                                                                                                                                                                                                                                                                                                                       |
| Positions    | DocPosition[] | +        | Элементы документа  | "Positions": [<br>{<br>"TextString": {<br>"Text": "[big]Мой дядя, самых честных правил,"<br>}<br>},<br>{<br>"TextString": {<br>"Text": "[dotted]"<br>}<br>},<br>{<br>"TextString": {<br>"Text": "Он уважать себя заставил,"<br>}<br>},<br>{<br>"Barcode": {<br>"Type": "EAN13",<br>"Barcode": "1235467890126"<br>}<br>}<br>] |

**Тело ответа**

**Result (SlipCheck)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ShiftNumber | int | + | Номер сессии | 0 |
| DocNumber | int | + | Номер фискального документа | 0 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 0 |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "0001-01-01T00:00:00" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00109325182732",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000001002520",<br>"Fn": "9999078902012910",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"CheckNumber": 20,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| Lines | [PrintLine](#printline)[] | - | Заголовок документа | "Lines": [<br>{<br>"Type": 1,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "Мой дядя, самых честных правил,",<br>"LineRight": "",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": true,<br>"IsCreateFromTemplate": false<br>},<br>{<br>"Type": 1,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "Он уважать себя заставил,",<br>"LineRight": "",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": true,<br>"IsCreateFromTemplate": false<br>},<br>{<br>"Type": 2,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "",<br>"LineRight": "",<br>"Alignment": 1,<br>"Font": 0,<br>"Wrap": true,<br>"Barcode": {<br>"Type": "EAN13",<br>"Value": "1235467890126",<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAHEAAAB",<br>"PrintText": 1,<br>"Height": 100,<br>"BarWidth": 2<br>},<br>"IsCreateFromTemplate": false<br>}<br>] |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 9 |
| DocId | string | + | Идентификатор нефискального документа. Передайте в [GET slip](#getslip), чтобы получить результат. | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |

### `PrintSlipAsync()`
`POST slip/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно поставить нефискальный документ в очередь печати и получить идентификатор задачи.

**Тело запроса**
Смотрите документацию [POST slip](#printslip)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | + | Идентификатор нефискального документа. Передайте в [GET slip](#getslip), чтобы получить результат. | "94cc354c-2dd3-4231-ac50-aa3a62883efe" |

### `GetSlip()`
`GET slip`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Возвращает нефискальный документ по идентификатору документа (docId), полученный в [POST slip](#printslip).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор нефискального документа | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (SlipCheck)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ShiftNumber | int | - | Номер сессии | 0 |
| DocNumber | int | - | Номер фискального документа | 0 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 0 |
| Fn | string | - | Серийный номер фискального накопителя | "" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "0001-01-01T00:00:00" |
| CashierName | string | - | Имя кассира | Иванов А.И. |
| CashierVatin | string | - | ИНН кассира | 7722345678 |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00109325182732",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000001002520",<br>"Fn": "9999078902012910",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 3,<br>"CheckNumber": 20,<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>} |
| DocId | string | + | Идентификатор нефискального документа. | "94cc354c-2dd3-4231-ac50-aa3a62883efe" |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 9 |
|
| Lines | [PrintLine](#printline)[] | - | Строки печати | "Lines": [<br>{<br>"Type": 1,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "Мой дядя, самых честных правил,",<br>"LineRight": "",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": true,<br>"IsCreateFromTemplate": false<br>},<br>{<br>"Type": 1,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "Он уважать себя заставил,",<br>"LineRight": "",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": true,<br>"IsCreateFromTemplate": false<br>},<br>{<br>"Type": 2,<br>"Width": 0,<br>"Scale": 100,<br>"Line": "",<br>"LineRight": "",<br>"Alignment": 1,<br>"Font": 0,<br>"Wrap": true,<br>"Barcode": {<br>"Type": "EAN13",<br>"Value": "1235467890126",<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAHEAAAB",<br>"PrintText": 1,<br>"Height": 100,<br>"BarWidth": 2<br>},<br>"IsCreateFromTemplate": false<br>}<br>] |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которго пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Emu" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |

### `GetSlipList()`
`GET slip/list`

**Заполняет:** `Checks` (те же списки: `Slips`, `CashIns`, `Corrections105`, `Corrections120`).

Получение списка нефискальных документов

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (SlipCheck[])**
Смотрите документацию [GET slip](#getslip)

---

## Картинки

### `SendPicture()`
`POST picture`

**Заполняет:** только статус.

Загрузка изображения в выбранную ККТ

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| Base64 | string | + | Изображение закодированное в Base64 | "iVBORw0KGgoAAAANSUhEUgA..." |
| PictureName | string | + | Название изображения — используется как идентификатор при последующем получении или удалении. | "logo" |
| Alignment | int | + | Выравнивание изображения при печати: <br>1 — по левому краю; <br>2 — по центру; <br>3 — по правому краю. Если не указано — 2 (по центру). | 2 |

**Тело ответа**

**Result (Picture)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| PictureNumber | int | - | Номер изображения | 18 |

### `GetPicture()`
`GET picture`

**Заполняет:** `PictureBase64Result`.

Получение изображения 

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Имя изображение. | name32 |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (Picture)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Picture | string | - | Изображение в кодировке Base64 | "iVBORw0KGgoAAAANSUhEUg..." |

### `GetPictureList()`
`GET picture/list`

**Заполняет:** `Pictures`.

Получение списка изображений

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (Picture[])**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| PictureBase64 | string | + | Изображение в формате base64 | "iVBORw0KGgoAAAANSU..." |
| PictureName | string | + | Название изображения | "name" |
| Width | int | - | Ширина изображения при печати, в точках | 200 |
| Height | int | - | Высота изображения при печати, в точках | 80 |
| StartLineNumber | int | - | Номер первой строки печати изображения | 0 |
| EndLineNumber | int | - | Номер последней строки печати изображения | 0 |
| Alignment | int | - | Выравнивание: <br>1 - по левому краю;  <br>2 - по центру;  <br>3 - по правому краю. | 2 |
| IsUploaded | bool | - | Признак загрузки изображения в память ККТ | false |
| Override | bool | - | Признак перезаписи изображения в памяти ККТ | false |

### `DeletePicture()`
`DELETE picture`

**Заполняет:** только статус.

Удаление картинки с устройства

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |
| DocumentId | string | + | Имя изображение | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

---

## Рекламные шаблоны

### `AddTemplate()`
`POST template`

**Заполняет:** `TemplateName` (имя, присвоенное сервером).

Создание печатного шаблона

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | + | Имя шаблона. Используется как уникальный идентификатор шаблона на сервере. Имя должно быть уникальным — создание шаблона с уже существующим именем не допускается. Поле является обязательным и не может быть пустым. Разрешённые символы: a-zA-Z0-9_-(). Использование пробелов не допускается.| "name120" |
| Type | int | + | Тип шаблона: <br>0 — Реклама;  <br>1 — Строки чека;  <br>2 — Шапка или подвал чека | 1 |
| TemplateItems | [AdvItem](#advitem)[] | + | Строки шаблона (только нефискальные поля) | "TemplateItems": [<br>{<br>  "PrintLine": {<br>"Type": 1,<br>"Line": "Текст",<br>"LineRight": "",<br>"Alignment": 1,<br>"Font": 5,<br>"Wrap": true<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 1,<br>"Line": "Сумма",<br>"LineRight": "1 250,00",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": false<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 4,<br>"SeparatorLine": {<br>"LineStyle": 0<br>}<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 2,<br>"Alignment": 1,<br>"Barcode": {<br>"Type": "QR",<br>"Value": "https://www.rbsoft.ru/",<br>"PrintText": 0,<br>"Height": 30,<br>"BarWidth": 6<br>}<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 3,<br>"Alignment": 1,<br>"Scale": 100,<br>"Picture": {<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",<br>"Alignment": 2,<br>"Width": 200,<br>"Height": 80<br>}<br>}<br>}<br>] |

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | - | Название шаблона. Данное значение можно использовать для GET или [DELETE template](#deletetemplate), чтобы получить более подробную информацию или удалить созданный печатный шаблон. | "name7" |

### `UpdateTemplate()`
`PUT template`

**Заполняет:** только статус.

Редактирование печатного шаблона по его уникальному названию (Name)

**Тело запроса**
Смотрите документацию [POST template](#addtemplate)

### `DeleteTemplate()`
`DELETE template`

**Заполняет:** только статус.

Удаление шаблона по названию

**Параметры запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Название печатного шаблона | base_header |

### `GetTemplateList()`
`GET template/list`

**Заполняет:** `Templates` (`TemplateNames`).

Получение списка печатных шаблонов

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string[] | - | Массив с названиями шаблонов | "base_advertisement",  <br>"base_check_lines",  <br>"base_header",  <br>"name3",  <br>"name7",  <br>"test" |

### `GetTemplate()`
`GET template`

**Заполняет:** `PrintTemplate…`, `TemplateName`, `TemplateType`, `TemplateLines`; строки шаблона восстанавливаются во входные позиции.

Получение печатного шаблона

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| name | string | + | Название шаблона. | base_header |

**Тело ответа**

**Result (TemplateParameters)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | + | Уникальное имя шаблона. | "base_header" |
| TemplateItems | [AdvItem](#advitem)[] | + | Строки шаблона (только нефискальные поля) | "TemplateItems": [<br>{<br>  "PrintLine": {<br>"Type": 1,<br>"Line": "Текст",<br>"LineRight": "",<br>"Alignment": 1,<br>"Font": 5,<br>"Wrap": true<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 1,<br>"Line": "Сумма",<br>"LineRight": "1 250,00",<br>"Alignment": 0,<br>"Font": 0,<br>"Wrap": false<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 4,<br>"SeparatorLine": {<br>"LineStyle": 0<br>}<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 2,<br>"Alignment": 1,<br>"Barcode": {<br>"Type": "QR",<br>"Value": "https://www.rbsoft.ru/",<br>"PrintText": 0,<br>"Height": 30,<br>"BarWidth": 6<br>}<br>}<br>},<br>{<br>"PrintLine": {<br>"Type": 3,<br>"Alignment": 1,<br>"Scale": 100,<br>"Picture": {<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",<br>"Alignment": 2,<br>"Width": 200,<br>"Height": 80<br>}<br>}<br>}<br>] |
| Type | int | - | Тип шаблона: <br>0 — Реклама;  <br>1 — Строки чека;  <br>2 — Шапка или подвал чека | 1 |

---

## Шаблоны чека

### `AddCheckTemplate()`
`POST checkTemplate`

**Заполняет:** только статус.

Создание нового шаблона чека

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | + | Уникальное имя шаблона чека. Допустимы только символы "a-zA-Z0-9_-()", пробелы запрещены. | "sale_template_01" |
| Document | CheckTemplateDocumentParameters | + | Данные документа шаблона. | "Document": {<br>"PaymentType": 1,<br>"TaxVariant": 0,<br>"Payments": { "Cash": 101 },<br>"Positions": [<br>{<br>"FiscalString": {<br>"Name": "Товар",<br>"Quantity": 1,<br>"PriceWithDiscount": 101,<br>"SumWithDiscount": 101,<br>"Tax": "0",<br>"SignMethodCalculation": 4,<br>"SignCalculationObject": 33<br>}<br>}<br>]<br>} |

**CheckTemplateDocumentParameters**
Смотрите документацию [POST check](#printcheck)

### `UpdateCheckTemplate()`
`PUT checkTemplate`

**Заполняет:** только статус.

Редактирование шаблона чека

**Тело запроса**
Смотрите документацию [POST checkTemplate](#addchecktemplate)

### `DeleteCheckTemplate()`
`DELETE checkTemplate`

**Заполняет:** только статус.

Удаление шаблона чека по имени


**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Название шаблона чека | piot_test_classic_5.1 |

### `GetCheckTemplateList()`
`GET checkTemplate/list`

**Заполняет:** `CheckTemplates`.

Получение списка шаблонов чека

**Тело ответа**

**Result (CheckTemplate[])**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | + | Уникальное имя шаблона чека. | "piot_test_classic_5.1" |
| TaskType | int | + | Тип чека.  <br>0 — Текст;  <br>1 — Продажа;  <br>2 — Возврат;  <br>3 — Покупка;  <br>4 — ВозвратПокупки;  <br>5 — ЧекКоррекцииПрихода;  <br>6 — ЧекКоррекцииВозвратаПрихода;  <br>7 — ЧекКоррекцииРасхода;  <br>8 — ЧекКоррекцииВозвратаРасхода | 1 |

### `GetCheckTemplate()`
`GET checkTemplate`

**Заполняет:** `CheckTemplate…`, `TemplateName`; документ шаблона восстанавливается во входные свойства чека (позиции, оплаты, СНО, адрес).

Получение шаблона чека по имени

**Параметры запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Имя шаблона чека | piot_test_classic_5.1 |

**Тело ответа**

**Result (CheckTemplate)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | + | Уникальное имя шаблона чека. | "piot_test_classic_5.1" |
| Document | [CheckTemplateDocument](#checktemplatedocument) | - | Сохранённые данные документа шаблона | "Document": {<br>"TaskType": 1,<br>"TaxType": 0,<br>"TimeZone": 0,<br>"OperationOnline": false,<br>"IsReplaceTax": true,<br>"Sum": 101,<br>"Electronically": false,<br>"IsFiscal": false,<br>"MtNumber": 0,<br>"PrintError": false,<br>"CheckItems": [<br>{<br>"Name": "Молоко Сценарий 1, 2, 5, 7, 14, Авария",<br>"Quantity": 1,<br>"Price": 101,<br>"Sum": 101,<br>"IsFiscal": true,<br>"MarkingCode": "0104670540176099215'W9Um93dGVz",<br>"ProductCode": "4670540176099"<br>}<br>],<br>"Payments": {<br>"Cash": 101,<br>"Electronic": 0,<br>"PrePaid": 0,<br>"Credit": 0,<br>"Barter": 0<br>}<br>} |

---

## Маркировка

### `OpenSessionRegistrationKM()`
`POST marking/session/open`

**Заполняет:** только статус.

Открытие сессии регистрации (проверки) кодов маркировки на ККТ.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Название устройства | "Atol" |

**Тело ответа**

**Result (BooleanResponse)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Code | int | - | Код ошибки | 0 |
| Description | string | - |Описание результата | "Ok" |
| Success | bool | - |Признак успешного открытия сессии. <br><br>`true` — сессия открыта. | true |

### `CloseSessionRegistrationKM()`
`POST marking/session/close`

**Заполняет:** только статус.

Закрытие сессии регистрации (проверки) кодов маркировки на ККТ.

**Тело запроса**
Смотрите документацию [POST marking/session/open](#opensessionregistrationkm)

**Тело ответа**

**Result (BooleanResponse)**
Смотрите документацию [POST marking/session/open](#opensessionregistrationkm)

### `RequestKM()`
`POST marking/km/request`

**Заполняет:** `MarkingCheck…`, `RequestKmGuid` (если не задан — создаётся перед запросом).

Локальная проверка кода маркировки на ККТ (ФФД 1.2).

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Название устройства | "Atol" |
| RequestKM      | [RequestKM](#requestkm-1) | +        | Параметры проверяемого кода маркировки. | "RequestKM": {<br>"Guid": "3a9b9d1f-c14b-474b-99f3-e09ee9c87620",<br>"NotSendToServer": true,<br>"WaitForResult": true,<br>"MarkingCode": "MDEwNDY3MDU0MDE3NjA5OTIxNSdXOVVtHTkzZEdWeg==",<br>"PlannedStatus": 0,<br>"Quantity": 0,<br>"MeasureOfQuantity": 0,<br>"FractionalQuantityNumerator": 0,<br>"FractionalQuantityDenominator": 0<br>} |

**Тело ответа**

**Result (RequestKMResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| ISMConnected | bool | - | Признак наличия связи с ОИСМ на момент отправки запроса | true |
| FormatChecking | bool | - | Признак того, что проверка формата кода маркировки прошла успешно | true |
| Checking | bool | - | Признак того, что проверка кода маркировки поставлена в обработку | true |
| CheckingResult | bool | - | Результат проверки, если он уже доступен на момент ответа. | false |
| Code | int | - | Код ошибки. <br><br>0 — Проверка была проведена успешна | 0 |
| Description | string | - | Описание | "Ошибок нет" |

### `GetProcessingKMResult()`
`GET marking/km/result`

**Заполняет:** `MarkingProcessing…`, `RequestKmGuid`.

Получение результата проверки кода маркировки в ОИСМ.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |

**Тело ответа**

**Result (ProcessingKMResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | bool | - | Итог проверки кода маркировки | false |
| ResultCode | int | - |  Код результата проверки (тег 2106 ФФД) | 5 |
| HandleCode | int | - | Код обработки запроса (тег 2105 ФФД) | 2 |
| RequestStatus | int | - | Статус получения результата от ОИСМ: <br>0 — результат получен; <br>1 — результат ещё не получен; <br>2 — результат не может быть получен. | 0 |
| Code | int | - | Код ошибки. <br><br>0 — Проверка была проведена успешна | 0 |
| Description | string | - | Описание | "Ошибок нет" |

### `ConfirmKM()`
`POST marking/km/confirm`

**Заполняет:** только статус.

Подтверждение, будет ли ранее проверенный код маркировки фактически включён в документ реализации. Подтверждение действительно только в рамках открытой сессии регистрации кодов маркировки.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |
| GUID | string | + | Код запроса. Его ранее передавали в marking/km/request. | "46dc63e5-8efa-4da6-b8ee-d6188da7b26a" |
| ConfirmationType | int | + | Признак подтверждения: <br>0 — код маркировки включён в документ реализации; <br>1 — код маркировки не включён. <br><br>Если поле не передано в запросе, оно принимает значение 0 — то есть отсутствие поля равносильно явному подтверждению продажи. | 1 |

**Тело ответа**

**Result (BooleanResponse)**
Смотрите документацию [POST marking/session/open](#opensessionregistrationkm)

### `VerifyMarking()`
`POST marking/km/verify`

**Заполняет:** `Marking…` (`MarkingCodesInfo`, `MarkingResult…`, `MarkingReq…`, `MarkingLast…`).

Проверка кодов маркировки. Метод проверки выбирается в настройках устройства, во вкладке "Маркировка", категория "Способ проверки маркировки". <br><br> Если выбрана проверка "Автоматически", сервер последовательно пробует ТС ПИоТ (если для устройства настроен адрес), затем ГИС МТ (если настроена учётная запись ОФД), затем локальный модуль ЧЗ (если он готов), и использует первый доступный способ. <br><br>По умолчанию используется ГИС МТ и ЛМ ЧЗ. 

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Название устройства | "Atol" |
| Codes | string[] | + | Массив проверяемых кодов маркировки в кодировке Base64. | ["MDEwNDY3MDU0MDE3NjA5OTIxNSdXOVVtHTkzZEdWeg=="] |

**Тело ответа**

**Result (CheckCodeMarkAnswer)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| code           | int | - | Код результата обращения к API «Честного Знака» | 0 |
| description    | string | - | Описание результата | "OK" |
| reqId | string | + | Идентификатор запроса в сервисе «Честного Знака» | "a1b2c3d4-e5f6-7890-abcd-ef1234567890" |
| reqTimestamp | long | - | Время выполнения проверки КМ (Unix-время в миллисекундах) | 1781076703146 |
| status | int | - | Статус локального модуля ЧЗ, используемого ТС ПИоТ для офлайн-проверки: <br>0 — unknown(неизвестно); <br>1 — ready(готов); <br>2 — initialization(инициализован); <br>3 — failure(ошибка); <br>4 — not_configured(не настроен). | "OK" |
| requiresDownload | bool | - | Признак необходимости скачать или обновить локальную базу данных модуля ЧЗ перед офлайн-проверкой | false |
| lastUpdate | long | - | Время последнего обновления локальной базы данных модуля. <br>0 — нет данных / не актуально. | 0 |
| lastSync | long | - | Время последней синхронизации модуля с ГИС МТ. <br>0 — нет данных. | 0 |
| codes | [CodeMarkInfo](#codemarkinfo)[] | + | Данные проверки кода маркировки. | "codes": [<br>{<br>"Cis": "0104670540176099215'W9Um93dGVz",<br>"valid": true,<br>"verified": true,<br>"realizable": true,<br>"utilised": true,<br>"found": true,<br>"sold": true,<br>"isBlocked": true,<br>"isGreyGtin": true,<br>"gtin": "04670540176099",<br>"groupIds": [8]<br>}<br>] |

### `VerifyMarkingTsPiot()`
`POST marking/km/tspiot/verify`

**Заполняет:** `Marking…` (`MarkingCodesInfo`, `MarkingResult…`, `MarkingReq…`, `MarkingLast…`).

Проверка кодов маркировки через ТС ПИоТ. <br><br>Для работы запроса на устройстве должен быть настроен адрес ТС ПИоТ

**Тело запроса**
Смотрите документацию [POST marking/km/verify](#verifymarking)

**Тело ответа**

**Result (CheckCodeMarkAnswer)**
Смотрите документацию [POST marking/km/verify](#verifymarking)

### `VerifyMarkingLmcz()`
`POST marking/km/lmcz/verify`

**Заполняет:** `Marking…` (`MarkingCodesInfo`, `MarkingResult…`, `MarkingReq…`, `MarkingLast…`).

Проверка кодов маркировки через локальный модуль "Честного Знака" (ЛМ ЧЗ).

**Тело запроса**
Смотрите документацию [POST marking/km/verify](#verifymarking)

**Тело ответа**

**Result (CheckCodeMarkAnswer)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| code                      | int                     | - | Код результата проверки (0 — без ошибок)                                                                                        | 0            |
| description               | string                  | - | Описание результата                                                                                                            | "OK"         |
| reqId                     | string                  | + | Идентификатор запроса                                                                                                          | "b0bbb45a-e773-8b63-32e6-da06f1002466" |
| reqTimestamp              | long                    | - | Время выполнения проверки кода маркировки (Unix-время в миллисекундах)                                                          | 1781077845362 |
| isCheckedOffline          | bool                    | - | Признак офлайн-проверки через локальный модуль                                                                                 | true         |
| status                    | int                    | - | Статус локального модуля ЧЗ: <br>0 — unknown; <br>1 — ready; <br>2 — initialization; <br>3 — failure; <br>4 — not_configured.   | 0            |
| requiresDownload          | bool                    | - | Признак необходимости скачать или обновить локальную базу данных модуля перед офлайн-проверкой                                  | false        |
| lastUpdate                | long                    | - | Время последнего обновления локальной базы данных модуля. 0 — данных нет или неактуально.                                       | 0            |
| lastSync                  | long                    | - | Время последней синхронизации модуля с ГИС МТ. 0 — данных нет.                                                                  | 0            |
| version                   | string                  | - | Версия локального модуля                                                                                                       | "019e978f-e682-7b78-8127-e7cf95b63586" |
| inst                      | string                  | - | Идентификатор экземпляра локального модуля ЧЗ                                                                                   | "019eb008-c2d2-7889-b91a-a3455dc42384" |
| serviceUrl                | string                  | - | Адрес локального модуля ЧЗ, к которому фактически было выполнено обращение | "127.0.0.1:5995" |
| codes | [CodeMarkInfo](#codemarkinfo)[] | + | Данные проверки кода маркировки | "codes": [<br>{<br>"cis": "0104670540176099215'W9Um",<br>"valid": false,<br>"verified": false,<br>"realizable": false,<br>"utilised": false,<br>"isTracking": false,<br>"sold": false,<br>"gtin": "04670540176099",<br>"grayZone": false,<br>"isBlocked": false,<br>"isGreyGtin": false,<br>"found": false,<br>"packageQuantity": 0<br>}<br>] |

---

## Фискализация

### `Fiscalization()`
`POST fiscalization`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`.

Выполнение фискализации

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Название устройства | "Atol" |
| OperationType | int | + | Тип операции:  <br>1 — регистрация;  <br>2 — изменение параметров;  <br>3 — закрытие ФН | 2 |
| Cashier               | [Cashier](#cashier) | –  | Сведения о кассире, выполняющем операцию. <br><br>Если тип данных **Cashier** отсутствует или все его поля пустые, запрос завершается ошибкой. | {"Name": "Иванов А.И.", "Vatin": "7722345678"} |
| FfdVersionFn | string | + | Версия ФФД ФН: <br>"1.0", <br>"1.1", <br>"1.2" | "1.2" |
| KKTNumber | string | + | Регистрационный номер ККТ | "0000000000047797" |
| KKTSerialNumber | string | + | Заводской номер ККТ | "0392790042005043" |
| Fn | string | + | Заводской номер ФН | "0123123123123" |
| CompanyName | string | + | Название организации | "ООО 'Ромашка'" |
| Vatin | string | + | ИНН организации | "7722345678" |
| SaleAddress | string | + | Адрес расчётов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | + | Место расчётов | "Офис" |
| TaxationSystems | string | + | Коды СНО через запятую. <br>0 — ОСН; <br>1 — УСН доход; <br>2 — УСН доход - расход; <br>3 — ЕНВД; <br>4 — ЕСХН; <br>5 — ПСН | "0,1,2,4,5" |
| FfdVersionKkt | string | + | Версия ФФД ККТ: "1.0", "1.0.5", "1.1", "1.2" | "1.2" |
| IsFiscal | bool | + | Фискальный режим | true |
| RegistrationLabelCodes | string | + | Коды причин изменения через ".". 0 — Замена фискального накопителя;<br> 1 — Замена оператора фискальных данных;<br> 2 — Изменение наименования пользователя контрольно-кассовой техники;<br> 3 — Изменение адреса и (или) места установки (применения) контрольно-кассовой техники;<br> 4 — Перевод ККТ из автономного режима в режим передачи данных;<br> 5 — Перевод ККТ из режима передачи данных в автономный режим;<br> 6 — Изменение версии модели ККТ;<br> 7 — Изменение перечня систем налогообложения, применяемых при осуществлении расчетов;<br> 8 — Изменение номера автоматического устройства для расчетов, в составе которого применяется ККТ;<br> 9 — Перевод ККТ из автоматического режима в неавтоматический режим (осуществление расчетов кассиром);<br> 10 — Перевод ККТ из неавтоматического режима (осуществление расчетов кассиром) в автоматический режим;<br> 11 — Перевод ККТ из режима, не позволяющего формировать БСО, в режим, позволяющий формировать БСО;<br> 12 — Перевод ККТ из режима, позволяющего формировать БСО, в режим, не позволяющий формировать БСО;<br> 13 — Перевод ККТ из режима расчетов в сети Интернет (позволяющего не печатать кассовый чек и БСО) в режим, позволяющий печатать кассовый чек и БСО;<br> 14 — Перевод ККТ из режима, позволяющего печатать кассовый чек и БСО, в режим расчетов в сети Интернет (позволяющего не печатать кассовый чек и БСО);<br> 17 — Перевод ККТ из режима, позволяющего применять ККТ при приеме ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению азартных игр, в режим, не позволяющий применять ККТ при приеме ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению азартных игр;<br> 18 — Перевод ККТ из режима, не позволяющего применять ККТ при приеме ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению азартных игр, в режим, позволяющий применять ККТ при приеме ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению азартных игр;<br> 19 — Перевод ККТ из режима, позволяющего применять ККТ при приеме денежных средств при реализации лотерейных билетов, электронных лотерейных билетов, приеме лотерейных ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению лотерей, в режим, не позволяющий применять ККТ при приеме денежных средств при реализации лотерейных билетов, электронных лотерейных билетов, приеме лотерейных ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению лотерей;<br> 20 — Перевод ККТ из режима, не позволяющего применять ККТ при приеме денежных средств при реализации лотерейных билетов, электронных лотерейных билетов, приеме лотерейных ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению лотерей, в режим, позволяющий применять ККТ при приеме денежных средств при реализации лотерейных билетов, электронных лотерейных билетов, приеме лотерейных ставок и выплате денежных средств в виде выигрыша при осуществлении деятельности по проведению лотерей;<br> 21 — Изменение версии ФФД;<br> 31 — Иные причины;<br> | "3.1" |
| ReasonCode | int | \- | Код причины перерегистрации (только для OperationType=2).<br>1 - Замена ФН,<br> 2 - Замена ОФД,<br> 3 - Изменение реквизитов,<br> 4 - Изменение настроек ККТ | 3 |
| OfdName | string | - | Название ОФД | "Тестовый ОФД" |
| OfdVatin | string | - | ИНН ОФД | "1234554321" |
| FnsUrl | string | - | Адрес сайта ФНС | "nalog.ru" |
| SenderEmail | string | - | Email отправителя чека | "ivanov@mail.ru" |
|IsEncrypted|	bool|	-	|Шифрование данных|	false|
|IsOffline|	bool|	-	|Автономный режим|	false|
|IsMarking	|bool	| - |	Торговля маркированными товарами|	true|
|IsOnline	|bool| - |	Расчеты только в интернет	|false|
|IsExcisable	|bool	| - | Продажа подакцизных товаров	|true|
|IsBsoSign	|bool| - 	Формирование АС БСО	|false|
|IsService	|bool	| - |Расчеты за услуги	|false|
|IsGambling	|bool|	—	|Проведение азартных игр	|false|
|IsLottery	|bool	|—	|Проведение лотерей	|false|
|IsAutomaticPrinter	|bool| - |Установка принтера в автомате	|false|
|IsAutomatic	|bool	| - |Автоматический режим	|false|
|IsPawnshop	|bool	| - |	Ломбардная деятельность	|false|
|IsAssurance	|bool	| - |Страховая деятельность	|false|
|IsCateringServices	|bool| - |Общественное питание	|false|
|IsWholesaleTrade	|bool	| - |Оптовая торговля	|false|
|IsVending	|bool	| - |Торговый автомат	|false|
| AgentTypes | string | - | Коды признаков агента через разделитель ",". <br> 0 — Банковский платежный агент;<br> 1 — Банковский платежный субагент;<br> 2 — Платежный агент;<br> 3 — Платежный субагент;<br> 4— Поверенный;<br> 5 — Комиссионер;<br> 6 — Агент;<br> | "1,2,4" |

**Тело ответа**

**Result (Fiscalization)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| OperationType | int | + | Тип выполненной операции | 2 |
| RnNumber | string | - | Регистрационный номер ККТ | "0000000000047797" |
| Date | DateTime | - | Дата и время выполнения операции | "2026-05-24T00:11:23.5221437+08:00" |
| IsBsoSign | bool | - | Формирование АС БСО | false |
| SenderEmail | string | - | Email отправителя чека | "ivanov@mail.ru" |
| ReasonCode | int | + | Код причины перерегистрации | 3 |
| IsMarking | bool | - | Торговля маркированными товарами | true |
| IsPawnshop | bool | - | Признак применения при осуществлении ломбардами кредитования граждан | false |
| IsAssurance | bool | - | Признак применения при осуществлении деятельности по страхованию | false |
| FnsUrl | string | - | Адрес сайта ФНС | "[nalog.ru"](http://www.nalog.gov.ru) |
| OfdVatin | string | - | ИНН ОФД | "1234554321" |
| OfdName | string | - | Название ОФД | "Тестовый ОФД" |
| IsAutomatic | bool | - | Признак автоматического режима | false |
| IsVending | bool | - | Признак применения в торговом автомате | false |
| IsAutomaticPrinter | bool | - | Признак установки принтера в автомате | false |
| IsOnline | bool | - | Признак ККТ для расчетов в Интернет | false, |
| IsLottery | bool | - | Признак проведения лотереи | false, |
| IsGambling | bool | - | Признак проведения азартных игр | false, |
| IsExcisable | bool | - | Признак продажи подакцизного товара | false, |
| IsService | bool | - | Признак расчетов за услуги | false, |
| IsEncrypted | bool | - | Признак шифрование данных | false, |
| IsOffline | bool | - | Признак автономного режима | false, |
| TaxationSystems | string | - | Коды системы налогообложения | "0,1,2,4,5", |
| Vatin | string | - | ИНН организация | "7722345678", |
| CompanyName | string | - | Название организации | "ООО 'Ромашка'", |
| FfdVersionKkt | string | - | Версия ФФД ККТ | "1.2" |
| FfdVersionFn | string | - | Версия ФФД ФН | "1.2 |
| IsSupportFnPiot | bool | - | ККТ поддерживает работу с ТС ПИоТ | false, |
| IsFiscal | bool | - | Фискальный режим | true |
| SerialNumber | string | - | Заводской номер ККТ | "0020260207", |
| RegistrationLabelCodes | string | - | Коды причин изменения сведений о ККТ | "3.1", |
| IsCateringServices | bool | - | Признак автономного режима | false, |
| IsWholesaleTrade | bool | - | Признак применения о оптовой торговле с организациями и ИП | false, |
| OfdPort | int | - | Порт для ФД отправки в ОФД | 0, |
| ShiftNumber | int | - | Номер сессии | 3 |
| DocNumber | int | - | Номер фискального документа | 5 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 1 |
| FiscalSign | string | - | Фискальный признак документа | "4988644533" |
| CashierName | string | - | Имя кассира | "Иванов А.И." |
| CashierVatin | string | - | ИНН кассира | "7722345678" |
| DocumentHeader | [DocumentHeader](#documentheader) | - | Заголовок документа | Заголовок фискального документа | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "00106305393630",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "0000000002005725",<br>"Fn": "9999078902010507",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 39,<br>"DocNumber": 343,<br>"FiscalSign": "1352614355",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "7709364346"<br>}  |
| Fn | string | - | Серийный номер фискального накопителя | "0123123123123" |
| FiscalDate | DateTime | - | Время регистрации операции по часам ККМ. | "2026-05-24T00:11:23.5505769+08:00" |
| SaleAddress | string | - | Адрес проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| Tlv | string | - | Cтруктура значений тегов документа | ""1209,Номер версии ФФД:4\\r\\n1041,Номер ФН:999907890..." |
| TaskType | int | - | Тип чека: <br>0 — Текст  <br>1 — Приход  <br>2 — Возврат прихода  <br>3 — Расход  <br>4 — Возврат расхода  <br>5 — Коррекция прихода  <br>6 — Коррекция возврата прихода  <br>7 — Коррекция расхода  <br>8 — Коррекция возврата расхода  <br>9 — Слип  <br>10 — Фискализация  <br>11 — Открытие смены  <br>12 — Z-отчет  <br>13 — X-отчет  <br>14 — Отчет о состоянии расчетов  <br>20 — Выемка  <br>21 — Внесение  <br>22 — Открытие денежного ящика | 10 |
| DocId | string | + | Идентификатор документа фискализации. Передайте в [GET fiscalization](#getfiscalization), чтобы получить результат | "7ccfb3d4-bc93-40c7-9e4d-aa1c8b0261b6" |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| TerminalId | string | - | Идентификатор терминала, с которого пришел документ | "" |
| DeviceName | string | + | Имя устройства | "Atol" |
| PoolId | string | - | Пул, который назначен чеку | "" |
| ResultCode | int | - | Результат | 0 |
| ResultDescription | string | - | Описания результата | "OK" |
| Processed | bool | - | Признак удачного завершения обработки | true |
| DeviceInfo | [DeviceInfo](#deviceinfo) | - | Данные о ККТ | "DeviceInfo": {<br>"FfdVersion": "1.2",<br>"FnFfdVersion": "1.2",<br>"TimeZone": 7,<br>"KktLicenses": [],<br>"IsFiscal": true,<br>"LineLength": 64,<br>"LineLengthPixels": 576,<br>"DeviceClass": 4,<br>"Model": "АТОЛ FPrint-22ПТК",<br>"SerialNumber": "00106305393630",<br>"FirmwareVersion": "5.15.102",<br>"ConfigurationVersion": "5.17.0"<br>} |
| ServerVersion | string | - | Версия сервера | "4.0.62.521" |

### `FiscalizationAsync()`
`POST fiscalization/async`

**Заполняет:** `DocumentId` — идентификатор задания; результат — через `GetTaskStatus` или `Get…` по этому docId.

Асинхронно поставить операцию фискализации в очередь

**Тело запроса**
Смотрите документацию [POST fiscalization](#fiscalization)

**Тело ответа**

**Result**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Result | string | - | Идентификатор документа фискализации. Передайте в [GET fiscalization](#getfiscalization)`, чтобы получить результат | "802e6783-6c95-42a2-b583-b99d6ca1b151" |

### `GetFiscalization()`
`GET fiscalization`

**Заполняет:** документ: свойства `Check…`, `DocumentId`, `FiscalSign`, `FnNumber`, `RnNumber`, `FnsUrl`, `ShiftNumber`, `CheckNumber`, `CheckNumberInShift`, `CashBalance`, `PrintForm`, `ServerDateTime`, `FiscalDateTime`, `DeviceDateTime`, `Device…`, `IsFiscal`; `FiscalizationResult…`, `RnNumber`, `IsFiscal`.

Возвращает результат фискализации по идентификатору документа (docId), полученному в [POST fiscalization](#fiscalization).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа фискализации | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (Fiscalization)**
Смотрите документацию [POST fiscalization](#fiscalization)

### `GetFiscalizationList()`
`GET fiscalization/list`

**Заполняет:** `Fiscalizations`.

Получение списка операций фискализации по устройству.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства. | Atol |

**Тело ответа**

**Result (Fiscalization[])**
Смотрите документацию [POST fiscalization](#fiscalization).

---

## Очередь

### `GetQueue()`
`GET queue`

**Заполняет:** `Queue`.

Получение текущего списка заданий, находящихся в очереди обработки сервера.

**Тело ответа**

**Result (ResponseQueue[])**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocId              | string       | + | Идентификатор документа задания. |
| DeviceName         | string       | + | Название устройства, которому адресовано задание | "Atol"  |
| PoolId | string       | - | Идентификатор пула, если задание адресовано пулу устройств, а не конкретному устройству | "" |
| SentToPrint        | int          | - | Состояние обработки задания: <br>0 — задание ещё не отправлено на устройство; <br>1 — задание отправлено на устройство и ожидает завершения; <br>2 — обработка завершилась ошибкой | 1 |
| Time | datetime | - | Время постановки задания в очередь | "2026-05-21T08:57:34"  |
| Printed | bool | - | Признак того, что задание уже успешно напечатано  | false  |
| Sum  | decimal | - | Сумма документа | 50 |
| ErrorDescription   | string | - | Текстовое описание текущего состояния или ошибки задания | "Постановка в очередь печати" |
| Session | int | - | Номер кассовой смены, к которой относится задание  | 0 |
| DocNumber | int | + | Номер документа (заполняется после успешной обработки)    | 0  |

### `GetQueueTask()`
`GET queue/task`

**Заполняет:** `QueueTask…`.

Получение актуального статуса задачи в очереди

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| taskId | string | + | Идентификатор документа (docId), полученный в ответе на POST-запрос. По нему задание отслеживается в очереди. | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (QueueStatus)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName               | string       | + | Название устройства                                                                                                                                        | "Atol"                                     |
| DocId                    | string         | + | Идентификатор документа. Данный идентификатор можно использовать для [GET queue/task/history](#getqueuetaskhistory), чтобы узнать историю обработки документа                                                                                                                                    | "53edb056-1bc0-458d-84d1-46e0cea211c1"    |
| DocState                 | int         | - | Код состояния документа                                                                                                                                    | 1                                           |
| QueueState               | int          | - | Код состояния очереди                                                                                                                                      | 1                                           |
| NumberInQueue            | int          | - | Позиция задания в очереди на момент запроса                                                                                                                 | 0                                           |
| ResultCode               | int          | - | Код результата. | 0                                           |
| ResultDescription        | string       | - | Описание результата                                                                                                                                         | "OK"                                        |
| Date                     | datetime     | - | Дата и время последнего изменения статуса задачи                                                                                                            | "2026-05-21T16:57:34"                     |
| FiscalSign               | string       | - | Фискальный признак документа. Заполняется только для успешно обработанных фискальных заданий.                                                               | "3327337010"                                |
| DocumentHeader           | [DocumentHeader](#documentheader)   | - | Заголовок документа. | "DocumentHeader": {<br>"OrganizationInfo": "ООО 'Ромашка'",<br>"SerialNumber": "0020260207",<br>"Vatin": "7722345678",<br>"Cashier": "Иванов А.И.",<br>"RnNumber": "00031415926",<br>"Fn": "0123123123123",<br>"FnsUrl": "nalog.ru",<br>"ShiftNumber": 39,<br>"DocNumber": 336,<br>"FiscalSign": "3327337010",<br>"OfdOrganizationName": "Тестовый ОФД",<br>"OfdVatin": "1234554321"<br>} |
| PrintStatusDescription   | string   | - | Описание текущего этапа обработки задания | "Чек успешно напечатан" |
| OutputParameters | [OutputParameters](#outputparameters) | - | Выходные параметры для документов | "OutputParameters": {<br>"DepartmentTotals": [],<br>"NumberOfChecks": 4,<br>"NumberOfDocuments": 4,<br>"Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>},<br>"ResourcesFn": 365,<br>"ShiftNumber": 56,<br>"CheckNumber": 490,<br>"ShiftClosingCheckNumber": 4,<br>"DateTime": "2026-08-06T09:11:31.6942673+08:00",<br>"ShiftState": 1,<br>"CashBalance": 3075.80,<br>"FnValidityDate": "2027-08-06T09:11:31.6942673+08:00",<br>"DocumentsCounter": 0<br>} |
| ShiftTotal | [ShiftTotal](#shifttotal) | - | Сменные итоги |"ShiftTotal": {<br>"IsCountersReaded": true,<br>"ShiftNumber": 56.0,<br>"CashDrawer": {<br>"Sum": 3075.80,<br>"Count": 47<br>},<br>"ShiftIncome": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Counters": {<br>"SumCorrection": 0.0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 1,<br>"Sum": 30.0,<br>"Payments": {<br>"Sum": 30.0,<br>"Cash": 30.0,<br>"Electronically": 30.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 5.00<br>}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0.0,<br>"Payments": {<br>"Sum": 0.0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {}<br>}<br>}<br>}|
| OverallTotals | [OverallTotals](#overalltotals) | - | Необнуляемые итоги | "OverallTotals": {<br>"DataLoaded": true,<br>"Sum": 0.0,<br>"Count": 0,<br>"Counters": {<br>"SumCorrection": 0.0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 542,<br>"Sum": 37305.04,<br>"Payments": {<br>"Sum": 37305.04,<br>"Cash": 37326.08,<br>"Credit": 9400.00,<br>"Electronically": 12029.54,<br>"Barter": 2350.00,<br>"Prepaid": 4700.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 2081.26,<br>"TaxVat_7": 1536.90<br>}<br>},<br>"SalesReturn": {<br>"Count": 52,<br>"Sum": 4945.55,<br>"Payments": {<br>"Sum": 4945.55,<br>"Cash": 4970.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 63.32,<br>"TaxVat_7": 294.30<br>}<br>},<br>"SalesCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"SalesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"Purchases": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesReturn": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesCorrection": {<br>"Count": 60,<br>"Sum": 5820.0,<br>"Payments": {<br>"Sum": 5820.0,<br>"Cash": 5820.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 70.02,<br>"TaxVat_7": 294.30<br>}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}<br>},<br>"CashDrawer": {<br>"Sum": 3075.80,<br>"Count": 47<br>}<br>} |

### `GetQueueTaskHistory()`
`GET queue/task/history`

**Заполняет:** `QueueTask…`, `OperationHistory`.

Получение истории событий обработки конкретного задания в очереди

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| taskId | string | + | Идентификатор документа (docId), полученный в ответе на POST-запрос. По нему задание отслеживается в очереди. | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (QueueTaskHistoryResult)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| History | [DocumentHistory](#documenthistory)[] | - | Хронология событий обработки задачи, от самого раннего к самому позднему |"History": [<br>{<br>"Time": "2026-05-21T16:57:25",<br>"State": 4,<br>"Description": "Добавление в очередь",<br>"Info": ""<br>},<br>{<br>"Time": "2026-05-21T16:57:34",<br>"State": 0,<br>"Description": "Начало выполнения",<br>"Info": ""<br>},<br>{<br>"Time": "2026-05-21T16:57:44",<br>"State": 1,<br>"Description": "Успешное завершение",<br>"Info": ""<br>}<br>] |

### `CancelQueueTask()`
`DELETE queue/task`

**Заполняет:** только статус.

Удаление задания из очереди

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| taskId | string | + | Идентификатор документа (docId), полученный в ответе на POST-запрос. По нему задание отслеживается в очереди. | b0ce9370-4435-49f8-a5e8-9bc267a26f77 |

**Тело ответа**

**Result (QueueStatus)**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocId | string | + |  Идентификатор документа | "593ab0d0-1fdd-4b11-85af-0e6c2f09284c" |
| DocState | int | - | Состояние документа после удаления | 5 |
| QueueState | int | - | Состояние очереди после удаления | 0 |
| ResultCode | int | - | Код результата операции | 1 |
| ResultDescription | string | - | Описание результата | "Документ удален из очереди" |
| NumberInQueue | int | - | Позиция в очереди на момент запроса | 0 |
| Date | datetime | - | Время записи о последнем статусе | "0001-01-01T00:00:00" |

---

## Операции

### `GetOperationLast()`
`GET operation/last`

**Заполняет:** `Operation…`, `DocumentId`.

Последняя операция из базы сервера с учётом фильтров.

**Параметры запроса**

| **Имя параметра**  | **Тип** | **Обяз** | **Назначение**                                                                                                                                                                                                                                                                                                                                                                               | **Пример** |
| ------------------ | ------- | -------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------- |
| OperationTaskType  | int     | –        | Тип задания:  <br>1 — Приход;  <br>2 — Возврат прихода;  <br>3 — Расход;  <br>4 — Возврат расхода;  <br>5 — Коррекция прихода;  <br>6 — Коррекция возврата прихода;  <br>7 — Коррекция расхода;  <br>8 — Коррекция возврата расхода;  <br>9 — Слип;  <br>11 — Открытие смены;  <br>12 — Z-отчёт;  <br>13 — X-отчёт;  <br>14 — Отчёт о состоянии расчётов;  <br>22 — Открытие денежного ящика | 1          |
| OperationProcessed | bool    | –        | Только обработанные операции (параметр `isProcessed`)                                                                                                                                                                                                                                                                                                                                        | true       |

> Состав ответа зависит от типа задания. Ниже — пример для чека прихода
> (`TaskType = 1`); у смен и отчётов вместо позиций и оплат возвращаются `OutputParameters`
> и `OfdStatus`, у денежного ящика — `DrawerNumber`.

**Тело ответа**

**Result ([DeviceTask](#devicetask))**

| **Имя поля**        | **Тип**                             | **Обяз** | **Назначение**                                                                                                                                                                                                                                                                                                                                                                              | **Пример**                                              |
| ------------------- | ----------------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------- |
| CheckItems          | [CheckItem](#checkitem)[]         | -        | Позиции документа (для чеков)                                                                                                                                                                                                                                                                                                                                                               | [{"Name": "Кофе американо", "Sum": 150.0, …}]           |
| TrustedInFn         | bool                                | -        | Документ подтверждён в ФН                                                                                                                                                                                                                                                                                                                                                                   | false                                                   |
| Change              | decimal                             | -        | Сдача                                                                                                                                                                                                                                                                                                                                                                                       | 0.0                                                     |
| Sum                 | decimal                             | -        | Итоговая сумма документа                                                                                                                                                                                                                                                                                                                                                                    | 150.0                                                   |
| OperationOnline     | bool                                | -        | Признак расчёта в интернете                                                                                                                                                                                                                                                                                                                                                                 | false                                                   |
| ClientContact       | string                              | -        | Контакт покупателя                                                                                                                                                                                                                                                                                                                                                                          | ""                                                      |
| Electronically      | bool                                | -        | Электронный чек                                                                                                                                                                                                                                                                                                                                                                             | false                                                   |
| IsFiscal            | bool                                | -        | Фискальный документ                                                                                                                                                                                                                                                                                                                                                                         | true                                                    |
| TaxType             | int                                 | -        | Система налогообложения                                                                                                                                                                                                                                                                                                                                                                     | 0                                                       |
| SenderEmail         | string                              | -        | Email отправителя чека                                                                                                                                                                                                                                                                                                                                                                      | "serverkkm@mail.ru"                                     |
| QrData              | [QrCheckData](#qrcheckdata)       | -        | Данные QR-кода чека                                                                                                                                                                                                                                                                                                                                                                         | {"Fd": 688, "Fp": "8924647256", "N": 1}                 |
| AdditionalAttribute | string                              | -        | Дополнительный реквизит чека (тег 1192)                                                                                                                                                                                                                                                                                                                                                     | ""                                                      |
| TimeZone            | int                                 | -        | Часовая зона                                                                                                                                                                                                                                                                                                                                                                                | 2                                                       |
| Payments            | [Payments](#payments)             | -        | Оплаты                                                                                                                                                                                                                                                                                                                                                                                      | {"Cash": 150.0, "Electronic": 0.0, …}                   |
| ShiftNumber         | int                                 | -        | Номер смены                                                                                                                                                                                                                                                                                                                                                                                 | 76                                                      |
| DocNumber           | int                                 | -        | Номер фискального документа                                                                                                                                                                                                                                                                                                                                                                 | 688                                                     |
| DocNumberInShift    | int                                 | -        | Номер документа в смене                                                                                                                                                                                                                                                                                                                                                                     | 5                                                       |
| FiscalSign          | string                              | -        | Фискальный признак                                                                                                                                                                                                                                                                                                                                                                          | "8924647256"                                            |
| Fn                  | string                              | -        | Номер фискального накопителя                                                                                                                                                                                                                                                                                                                                                                | "0123123123123"                                         |
| FiscalDate          | datetime                            | -        | Дата документа по ФН                                                                                                                                                                                                                                                                                                                                                                        | "2026-08-31T14:49:55+08:00"                             |
| DocumentHeader      | [DocumentHeader](#documentheader) | -        | Реквизиты организации, ККТ и ОФД                                                                                                                                                                                                                                                                                                                                                            | {"OrganizationInfo": "…", "RnNumber": "00031415926", …} |
| SaleAddress         | string                              | -        | Адрес расчётов                                                                                                                                                                                                                                                                                                                                                                              | "г.Улан-Удэ, ул.Балтахинова, д17Е"                      |
| SaleLocation        | string                              | -        | Место расчётов                                                                                                                                                                                                                                                                                                                                                                              | "Офис"                                                  |
| FfdVersion          | string                              | -        | Версия ФФД документа                                                                                                                                                                                                                                                                                                                                                                        | "1.2"                                                   |
| Tlv                 | string                              | -        | TLV-представление документа                                                                                                                                                                                                                                                                                                                                                                 | ""                                                      |
| TaskType            | int                                 | -        | Тип задания:  <br>1 — Приход;  <br>2 — Возврат прихода;  <br>3 — Расход;  <br>4 — Возврат расхода;  <br>5 — Коррекция прихода;  <br>6 — Коррекция возврата прихода; <br>7 — Коррекция расхода;  <br>8 — Коррекция возврата расхода;  <br>9 — Слип;  <br>11 — Открытие смены;  <br>12 — Z-отчёт;  <br>13 — X-отчёт;  <br>14 — Отчёт о состоянии расчётов;  <br>22 — Открытие денежного ящика | 1                                                       |
| DocId               | string                              | -        | Идентификатор документа                                                                                                                                                                                                                                                                                                                                                                     | "567e2bde-9fb2-4fde-bac4-ceb6aed54bb4"                  |
| Date                | datetime                            | -        | Дата операции                                                                                                                                                                                                                                                                                                                                                                               | "2026-08-31T14:49:55+08:00"                             |
| TerminalId          | string                              | -        | Идентификатор терминала                                                                                                                                                                                                                                                                                                                                                                     | ""                                                      |
| DeviceName          | string                              | -        | Имя устройства                                                                                                                                                                                                                                                                                                                                                                              | "Emu"                                                   |
| PoolId              | string                              | -        | Идентификатор пула                                                                                                                                                                                                                                                                                                                                                                          | ""                                                      |
| ResultCode          | int                                 | -        | Код результата (`0` — успех)                                                                                                                                                                                                                                                                                                                                                                | 0                                                       |
| ResultDescription   | string                              | -        | Описание результата                                                                                                                                                                                                                                                                                                                                                                         | "OK"                                                    |
| Processed           | bool                                | -        | Операция обработана                                                                                                                                                                                                                                                                                                                                                                         | true                                                    |
| ServerVersion       | string                              | -        | Версия сервера                                                                                                                                                                                                                                                                                                                                                                              | "4.0.70.827"                                            |
| DeviceInfo          | [DeviceInfo](#deviceinfo)         | -        | Сведения об устройстве                                                                                                                                                                                                                                                                                                                                                                      | {"Model": "РБ-Софт:Эмулятор ККМ", "LineLength": 64, …}  |

### `GetOperation()`
`GET operation`

**Заполняет:** `Operation…`, `DocumentId`.

Операция по идентификатору документа.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение**          | **Пример**                             |
| ----------------- | ------- | -------- | ----------------------- | -------------------------------------- |
| DocumentId        | string  | +        | Идентификатор документа | "8f733118-76ce-4c92-8678-760bb72d3f62" |

**Тело ответа** 
Смотрите документацию [`GetOperationLast`](#getoperationlast).

### `GetOperationHistory()`
`GET operation/history`

**Заполняет:** `OperationHistory`.

История обработки операции (постановка в очередь, отправка на ККТ, результат).

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение**          | **Пример**                             |
| ----------------- | ------- | -------- | ----------------------- | -------------------------------------- |
| DocumentId        | string  | +        | Идентификатор документа | "8f733118-76ce-4c92-8678-760bb72d3f62" |

**Тело ответа**

**Result (OperationHistoryItem[])**

| **Имя поля** | **Тип**                     | **Обяз** | **Назначение**                   | **Пример**               |
| ------------ | --------------------------- | -------- | -------------------------------- | ------------------------ |
| Time         | datetime                    | -        | Время события                    | "2026-08-28T11:42:45"    |
| State        | int                         | -        | Код состояния                    | 4                        |
| Description  | string                      | -        | Описание события                 | "Добавление в очередь"   |
| Document     | [DeviceTask](#devicetask) | -        | Состояние документа на этом шаге | {"CheckItems": [...], …} |

### `GetOperationTlv()`
`GET operation/tlv`

**Заполняет:** `OperationTlv`.

TLV-представление документа

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа (параметр `docId`) | "8f733118-76ce-4c92-8678-760bb72d3f62" |

**Тело ответа**

| **Имя поля** | **Тип** | **Обяз** | **Назначение**              | **Пример**                                                                            |
| ------------ | ------- | -------- | --------------------------- | ------------------------------------------------------------------------------------- |
| Result       | string  | +        | TLV-представление документа | "1209,Номер версии ФФД:4\r\n1041,Номер ФН:9999078902010507\r\n1040,Номер ФД:342\r\n…" |

### `GetOperationKm()`
`GET operation/km`

**Заполняет:** `OperationKm`.

Журнал кодов маркировки операции. Результат — в свойстве `OperationKm`.
Пустой массив, если в операции нет кодов маркировки.

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа (параметр `docId`) | "8f733118-76ce-4c92-8678-760bb72d3f62" |

**Тело ответа**

**Result (OperationKmRow[])**

| **Имя поля**         | **Тип**  | **Обяз** | **Назначение**            | **Пример**                                            |
| -------------------- | -------- | -------- | ------------------------- | ----------------------------------------------------- |
| Cis                  | string   | -        | Код маркировки (КиЗ)      | "0104607058926809215fY9s<"                            |
| CheckedAt            | datetime | -        | Время проверки            | "2026-08-21T17:52:42"                                 |
| PrintView            | string   | -        | Представление для печати  | "0104607058926809215fY9s<"                            |
| Message              | string   | -        | Сообщение проверки        | "Марка не найдена: [0104…] Код маркировки не найден." |
| CheckStatus          | int      | -        | Статус проверки           | 0                                                     |
| PositionName         | string   | -        | Наименование позиции      | "Бутылка с водой 1л."                                 |
| DocIds               | string[] | -        | Идентификаторы документов | ["ba78057f-…", "c5c0cef7-…"]                          |
| SalePrice            | long     | -        | Цена продажи (в копейках) | 75000                                                 |
| DeviceName           | string   | +        | Имя устройства            | "Emu"                                                 |
| MarkId               | string   | -        | Идентификатор марки       | "aa07b239-3541-460c-8000-71614dd2ebca"                |
| KmVerificationMethod | int      | -        | Способ проверки КМ        | 1                                                     |
| KmCheckInitiator     | int      | -        | Инициатор проверки КМ     | 2                                                     |

### `GetOperationRelated()`
`GET operation/related`

**Заполняет:** `RelatedOperations`.

Связанные операции (например, приход и его возврат)

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentId | string | + | Идентификатор документа (параметр `docId`) | "8f733118-76ce-4c92-8678-760bb72d3f62" |

**Тело ответа** 

**Result (RelatedOperations)**

 Смотрите документацию [`GetOperationLast`](#getoperationlast).

### `GetOperationList()`
`GET operation/list`

**Заполняет:** `Operations`.

Список операций за период

**Параметры запроса**

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| OperationFrom | DateTime | + | Начало периода (параметр `from`) | 2026-08-01 |
| OperationTo | DateTime | + | Конец периода (параметр `to`) | 2026-08-31 |

**Тело ответа**

**Result (OperationListItem[])**

| **Имя поля**     | **Тип**  | **Обяз** | **Назначение**                                    | **Пример**                             |
| ---------------- | -------- | -------- | ------------------------------------------------- | -------------------------------------- |
| DocId            | string   | +        | Идентификатор документа                           | "56e92dac-8e07-4934-9dfa-7060400888d0" |
| BaseDocId        | string   | +        | Идентификатор базового документа                  | "383f6fd3-7ec6-4ccb-9608-4426fa2439b5" |
| RequestId        | string   | -        | Идентификатор запроса                             | ""                                     |
| TerminalId       | string   | -        | Идентификатор терминала                           | ""                                     |
| DeviceName       | string   | +        | Имя устройства                                    | "Emu"                                  |
| PoolId           | string   | -        | Идентификатор пула                                | ""                                     |
| Date             | datetime | -        | Дата операции                                     | "2026-08-28T14:52:22"                  |
| CreatedAt        | datetime | -        | Время создания                                    | "2026-08-28T06:52:22"                  |
| UpdateAt         | datetime | -        | Время обновления                                  | "2026-08-28T06:52:22"                  |
| TaskType         | int      | -        | Тип задания (см. `TaskType` в `GetOperationLast`) | 9                                      |
| TaskName         | string   | -        | Название задания                                  | "Слип"                                 |
| Sum              | decimal  | -        | Сумма документа                                   | 0.0                                    |
| SessionNumber    | int      | -        | Номер смены                                       | 75                                     |
| DocNumberInShift | int      | -        | Номер документа в смене                           | 0                                      |
| DocNumber        | int      | -        | Номер фискального документа                       | 682                                    |
| FnDate           | datetime | -        | Дата документа по ФН                              | "2026-08-28T14:52:22"                  |
| FiscalSign       | string   | -        | Фискальный признак                                | "4118171897"                           |
| Fn               | string   | -        | Номер фискального накопителя                      | "0123123123123"                        |
| ClientContact    | string   | -        | Контакт покупателя                                | ""                                     |
| CashierName      | string   | -        | Кассир                                            | "Иванов А.И."                          |
| RnKKT            | string   | -        | Регистрационный номер ККТ                         | "00031415926"                          |
| ZnKKT            | string   | -        | Заводской номер ККТ                               | "0020260207"                           |

---

## Администрирование ККТ

### `RebootDevice()`
`POST kkt/reboot`

**Заполняет:** только статус.

Перезагрузка устройства.

**Тело запроса**

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DeviceName | string | + | Имя устройства | "Atol" |



## Типы данных

### Входные типы данных

#### Cashier

| **Имя поля** | **Тип** | **Обяз** | **Назначение**                                                                            | **Пример**    |
| --- | --- | --- | --- | --- |
| Name         | string  | -        | Имя кассира. <br><br>Если не указано — подставляется имя из настроек ККТ, иначе «Кассир». | "Иванов А.И." |
| Vatin        | string  | -        | ИНН кассира. <br><br>Если не указан — ИНН кассира в чеке не печатается.                   | "7722345678"  |

Обязателен для `POST correction105`,`POST correction105/async` и `POST fiscalization`, `POST fiscaliztion/async` иначе операция завершиться с ошибкой.

#### Customer

| **Имя поля**     | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Info             | string  | -        | Наименование организации или фамилия, имя, отчество (при наличии). <br><br>Если не указано — в чек не попадает.       | "ООО 'Рога и Копыта'"                         |
| Email            | string  | -        | Электронная почта покупателя. <br><br>Если не указана — используется **Phone** (если указан).                         | "[kuznicov@mail.ru](mailto:kuznicov@mail.ru)" |
| Vatin            | string  | -        | ИНН покупателя. <br><br>Если не указан — ИНН покупателя в чек не попадает.                                            | "500100732259"                                |
| Phone            | string  | -        | Номер телефона. <br><br>Если не указан — используется **Email** (если указан).                                        | "+79021456776"                                |
| DateOfBirth      | string  | -        | Дата рождения покупателя (клиента) в формате "DD.MM.YYYY". <br><br>Если не указана — в чек не попадает.               | 01.01.1990                                    |
| Citizenship      | string  | -        | Код страны (ОКСМ). <br><br>Если не указан — в чек не попадает.                                                        | "643"                                         |
| DocumentTypeCode | string  | -        | Числовой код вида документа, удостоверяющего личность (ФФД, Таблица 116). <br><br>Если не указан — в чек не попадает. | "21"                                          |
| DocumentData     | string  | -        | Данные документа, удостоверяющего личность. <br><br>Если не указаны — в чек не попадают.                              | "Серия: 60 18 Номер: 345678"                  |
| Address          | string  | -        | Адрес покупателя. <br><br>Если не указан — в чек не попадает. | "г.Иркутск, ул.Апельсиновая, д31А, офис 15"   |

#### ApiPayments

| **Имя поля**      | **Тип** | **Обяз** | **Назначение**                                                             | **Пример** |
| --- | --- | --- | --- | --- |
| Cash              | decimal | -        | Сумма наличной оплаты. <br><br>Если не указана — считается 0.              | 50         |
| ElectronicPayment | decimal | -        | Сумма безналичными средствами. <br><br>Если не указана — считается 0.      | 0          |
| AdvancePayment    | decimal | -        | Сумма предоплатой (зачетом аванса). <br><br>Если не указана — считается 0. | 0          |
| Credit            | decimal | -        | Сумма постоплатой (в кредит). <br><br>Если не указана — считается 0.       | 0          |
| CashProvision     | decimal | -        | Сумма встречным предоставлением. <br><br>Если не указана — считается 0.    | 0          |

#### Position / CheckTemplateDocumentParameters

| **Поле**      | **Тип**    | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| FiscalString  | [FiscalString](#fiscalstring) | -        | **Фискальная** позиция. Участвует в расчёте суммы чека и передаётся в ФН. **Name** обязателен. <br><br>Обязателен в фискальных операциях. | "FiscalString": { <br>"Name": "Бутылка с водой 1л.", <br>"Quantity": 1, <br>"PriceWithDiscount": 30, <br>"SumWithDiscount": 30, <br>"Tax": "20", <br>"SignMethodCalculation": 4, <br>"SignCalculationObject": 1, <br>"TaxSum": 0 <br>} |
| TextString    | [TextString](#textstring) | -        | **Нефискальная** текстовая строка — печатается в чеке, на сумму не влияет.  | "TextString": { <br>"Text": "[center]Спасибо за покупку!" <br>} |
| SeparatorLine | [SeparatorLine](#separatorline) | -        | **Нефискальная** горизонтальная линия на всю ширину чека. | "SeparatorLine": { <br>"lineStyle": 3 <br>}  |
| Barcode       | [ApiBarcode](#apibarcode) | -        | Штрихкод или QR-код. | "Barcode": { <br>"Type": "QR", <br>"Value": "https://roga-i-kopyta.ru", <br>"Alignment": "center" }                                                                                                                                   |
| Picture       | [Picture](#picture) | -        | **Нефискальная** картинка в кодировке Base64.                                                                                                                | "Picture": { <br>"Value": "iVBORw0KGgo...", <br>"Alignment": 1 } |

#### FiscalString

| **Имя поля**                | **Тип**    | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name                        | string     | +        | Название товара или услуги  | "Бутылка с водой 1л." |
| Quantity                    | decimal    | -        | Количество товара. <br><br>Если указано 0 или не указано — автоматически устанавливается 1. | 1  |
| ProductCode                 | string     | -        | Код товара  | "6404 11 000 0" |
| PriceWithDiscount           | decimal    | +        | Цена единицы товара с учетом скидок/наценок  | 30 |
| SumWithDiscount             | decimal    | +        | Конечная сумма по позиции чека с учетом всех скидок/наценок | 30 |
| Department                  | int        |          | Отдел, по которому ведется продажа | 0 |
| Tax                         | string     | +        | Ставка НДС | "20" |
| MeasurementUnit             | string     | +        | Единица измерения предмета расчета | "шт"|
| DiscountSum                 | string     | -        | Сумма скидок и наценок | 0 |
| TaxSum                      | decimal    | +        | Сумма НДС за предмет расчета | 0 |
| SignMethodCalculation       | int        | +        | Признак способа расчета: <br>0 — Не применяется; <br>1 — Предоплата полная; <br>2 — Предоплата частичная; <br>3 — Аванс; <br>4 — Полная оплата; <br>5 — Частичная оплата и кредит; <br>6 — Передача в кредит; <br>7 — Оплата кредита. <br><br>Если не указано или значение некорректно — по умолчанию **4** (полная оплата).  <br><br>Если в настройках ККТ, во вкладке "Автозамена" выставлен флаг "Заменять принудительно", значение признака предмета расчета и признака способа расчета из запроса заменяется значением из настроек ККТ. | 4 |
| SignCalculationObject       | int        | +        | Признак предмета расчета:<br>0 — Не применяется;<br>1 — Товар (товар, за исключением подакцизного товара); <br>2 — Подакцизный товар; <br>3 — Работа; <br>4 — Услуга; <br>5 — Ставка (приём ставок при деятельности по организации и проведению азартных игр); <br>6 — Выигрыш (выплата выигрышей в азартных играх); <br>7 — Лотерея (реализация лотерейных билетов или ставок при деятельности по организации и проведению лотерей); <br>8 — Выигрыш в лотерее (выплата выигрышей в лотереях); <br>9 — Право (прав на использование результатов интеллектуальной деятельности или средств индивидуализации); <br>10 — Аванс (аванс, задаток, предоплата, кредит, взнос в счет оплаты, пени, штраф, вознаграждение, бонус и иной аналогичный предмет расчета); <br>11 — Агентское вознаграждение (о вознаграждении пользователя, являющегося платежным агентом (субагентом), банковским платежным агентом (субагентом), комиссионером, поверенным или иным агентом); <br>12 — Выплата (о взносе в счет оплаты, пени, штрафе, вознаграждении, бонусе и ином аналогичном предмете расчета); <br>13 — Иной предмет расчета (о предмете расчета, не относящемуся к предметам расчета, которым может быть присвоено значение от "1" до "11" и от "14" до "26"); <br>14 — Имущественное право; <br>15 — Внереализационный доход; <br>16 — Иные платежи и взносы; <br>17 — Торговый сбор; <br>18 — Туристический налог; <br>19 — Залог; <br>20 — Расход; <br>21 — Взносы на ОПС ИП (взносы на обязательное пенсионное страхование ИП); <br>22 — Взносы на ОПС (взносы на обязательное пенсионное страхование); <br>23 — Взносы на ОМС ИП (взносы на обязательное медицинское страхование ИП); <br>24 — Взносы на ОМС (взносы на обязательное медицинское страхование);<br>25 — Взносы на ОСС (взносы на обязательное социальное страхование); <br>26 — Платеж казино (о приеме и выплате денежных средств при осуществлении казино и залами игровых автоматов расчетов с использованием обменных знаков игорного заведения — максимальное значение для ФФД 1.05); <br>27 — Выдача ДС (выдача денежных средств банковским платежным агентом); <br>30 — АТНМ (подакцизный товар, подлежащий маркировке средством идентификации, не имеющим кода маркировки); <br>31 — АТМ (подакцизный товар, подлежащий маркировке средством идентификации, имеющим код маркировки); <br>32 — ТНМ (товар, подлежащий маркировке средством идентификации, не имеющим кода маркировки, за исключением подакцизного товара); <br>33 — ТМ (товар, подлежащий маркировке средством идентификации, имеющим код маркировки, за исключением подакцизного товара); <br><br>Если не указано или значение некорректно — по умолчанию **1** (Товар). <br><br>Если в настройках ККТ, во вкладке "Автозамена" выставлен флаг "Заменять принудительно", значение признака предмета расчета и признака способа расчета из запроса заменяется из настроек ККТ. | 10 |
| SignSubjectCalculationAgent | int        | -        | Признак агента по предмету расчета: <br>0 — Банковский платежный агент; <br>1 — Банковский платежный субагент; <br>2 — Платежный агент; <br>3 — Платежный субагент; <br>4 — Поверенный; <br>5 — Комиссионер; <br>6 — Агент (иной тип). <br><br>Если не указан в позиции — используется значение **AgentSign** из заголовка чека.<br><br>Если не указан ни в позиции, ни в заголовке — признак агента в позиции не передаётся. | 5 |
| AgentData                   | [AgentData](#agentdata) | -        | Данные агента по позиции. <br><br>Если в позиции не указаны — используются данные **AgentData** из тела чека. <br><br>Если не указаны ни в позиции, ни в теле — данные агента не передаются. <br><br>Обязательно вместе с **SignSubjectCalculationAgent**. | "AgentData": { <br>"PayingAgentOperation": "Прием платежей", <br>"PayingAgentPhone": ["+79021654832"] <br>} |
| Vendor                      | [VendorData](#vendordata) | -        | Данные поставщика по позиции. <br><br>Если в позиции не указаны — используются данные **Vendor** из тела чека. <br><br>Если не указаны ни в позиции, ни в теле — данные поставщика не передаются. | "Vendor": { <br>"Phones": ["+79031234567"], <br>"Name": "ИП 'Ромашка'", <br>"Vatin": "5262107639" <br>} |
| GoodCodeData                | CommodityNomenclatureCode | -        | Данные кода товарной номенклатуры. <br><br>Если не указан — можно передать код маркировки через поле **MarkingCode**. <br><br>Если не указано ни **GoodCodeData**, ни **MarkingCode** — код товара в позиции не передаётся. | "GoodCodeData": { <br>"MarkingCode": "MDEwNDYxMDE0NDA4NTM0MjIxNSVvIS1MVEVsQ2dBTB05MUVFMDkdOTJtdTlXR3VGK0hyQlFCR3ZUMTNneVc0d29uRmVqZ2FZd1YzSkhkN0U1VHhnPQ==", <br>"Gtin": "04601234567890", <br>"StampType": "05" } |
| MarkingCode                 | string     | -        | Код контрольной марки. <br><br>Если не указан — код маркировки в позицию не передаётся (если не задан **GoodCodeData.MarkingCode**). | "MDEwNDYxMDE0NDA4NTM0MjIxNSVvIS1MVEVsQ2dBTB05MUVFMDkdOTJtdTlXR3VGK0hyQlFCR3ZUMTNneVc0d29uRmVqZ2FZd1YzSkhkN0U1VHhnPQ==" |
| AdditionalAttribute         | string     | -        | Дополнительный реквизит предмета расчет | "Доп информация: вода из Байкала" |
| CountryOfOrigin             | string     | -        | Цифровой код страны происхождения товара(ОКСМ) | "994"|
| CustomsDeclaration          | string     | -        | Регистрационный номер таможенной декларации | "06532/220211/0001122" |
| MeasureOfQuantity           | int        | -        | Мера количества предмета расчета. Значение из таблицы 114 (ФФД) | 10 |
| IndustryAttribute           | [IndustryAttribute](#industryattribute) |  -        | Отраслевой реквизит предмета расчёта. <br><br>Если не указан — в позицию не попадает. Отдельные поля внутри объекта необязательны. <br><br>Если для позиции передан код маркировки в **AttributeValue**, сервер считает, что проверка этого кода маркировки уже была выполнена внешней системой, и не выполняет собственную проверку кода при печати чека. | "IndustryAttribute": { <br>"IdentifierFoiv": "3", <br>"DocumentDate": "01.07.2026", <br>"DocumentNumber": "6", <br>"AttributeValue": "UUID=8f3a9d1c-7e2b-4a5f-9c8d-1e2f3a4b5c6d&Time=2088157392047" <br>} |
| ExciseAmount                | decimal    |          | Сумма акциза с учетом копеек | 1 |
| FractionalQuantity          | [FractionalQuantity](#fractionalquantity) |          | Дробное количество предмета расчёта. Задаётся числителем и знаменателем. | "FractionalQuantity": { <br>"Numerator": 208, <br>"Denominator": 1 <br>} |

#### CorrectionData

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример**              |
| --- | --- | --- | --- | --- |
| Type         | int     | -        | Тип коррекции: <br>0 — самостоятельно (коррекция по собственной инициативе); <br>1 — по предписанию (коррекция по предписанию налогового органа). <br><br>Если указано значение 1, рекомендуется заполнять поле **Number**. | 1  |
| Description  | string  | -        | Описание (основание) коррекции. | "Основание коррекции"  |
| Date         | DateTime| -        | Дата совершения корректируемого расчета. | "2026-03-30T00:00:00"  |
| Number       | string  | -        | Номер предписания налогового органа. Если коррекция самостоятельная (**Type** = 0), поле можно заполнить значением "0". | "0"  |

#### GoodCodeData

| **Имя поля**  | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| MarkingCode   | string  | -        | Код контрольной марки в кодировке Base64. <br><br>Если не указан — используется **MarkingCode** из **FiscalString** (если задан). | "MDEwNDYxMDE0NDA4NTM0MjIxNSVvIS1MVEVsQ2dBTB05MUVFMDkdOTJtdTlXR3VGK0hyQlFCR3ZUMTNneVc0d29uRmVqZ2FZd1YzSkhkN0U1VHhnPQ==" |
| Gtin          | string  | -        | GTIN. <br><br>Если не указан — в чек не попадает. | "04601234567890" |
| StampType     | string  | -        | Тип маркировки:<br>"02" — мех; <br>"05" — табак; <br>"1520" — обувь. <br><br>Если не указан — тег не передаётся. | "05" |
| Stamp         | string  | -        | КиЗ. <br><br>Если не указан — тег не передаётся. | "RU-ABC/1234567890123456" |
| SerialNumber  | string  | -        | Серийный номер. <br><br>Если не указан — тег не передаётся. | "5kX9mP2vQ7nR" |
| Barcode       | string  | -        | Штрихкод. <br><br>Если не указан — тег не передаётся. | "4601234567890" |
| NotIdentified | string  | -        | Код неидентифицированного формата в кодировке Base64. <br><br>Если не указан — не используется. | "U0tVLUxPQ0FMLTg4NDIx" |
| EAN8          | string  | -        | EAN-8 в кодировке Base64. <br><br>Если не указан — не используется.| "NDYwMTIzNDU=" |
| EAN13         | string  | -        | EAN-13 в кодировке Base64. <br><br>Если не указан — не используется. | "NDYwMTIzNDU2Nzg5MA==" |
| ITF14         | string  | -        | ITF-14 в кодировке Base64. <br><br>Если не указан — не используется. | "MTQ2MDEyMzQ1Njc4OTM=" |
| GS10          | string  | -        | GS10 (без маркировки) в кодировке Base64. <br><br>Если не указан — не используется. | "MDEwNDYwNDA2MDAwMzAxNQ==" |
| GS1M          | string  | -        | GS1 (с маркировкой) в кодировке Base64. <br><br>Если не указан — не используется. | "OTEwMjM0NTY3ODkwMTIzNDU2Nzg5MDEyMzQ1Ng==" |
| KMK           | string  | -        | Короткий код маркировки в кодировке Base64.<br><br>Если не указан — не используется. | "MDEwNDYwNDA2MDAwMzAxNTIxQWJDZEVm"|
| MI            | string  | -        | КиЗ мехового изделия. <br><br>Если не указан — не используется.  | "UlUtQUJDLzEyMzQ1Njc4OTAxMjM0NTY="  |
| EGAIS20       | string  | -        | ЕГАИС-2.0 в кодировке Base64. <br><br>Если не указан — не используется. | "MDEwMDAwMDAwMDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTIzNDU2Nzg5MDEyMzQ=" |
| EGAIS30       | string  | -        | ЕГАИС-3.0 в кодировке Base64. <br><br>Если не указан — не используется. | "MDMwMDAwMDAwMDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTIzNDU2Nzg5MDEyMzQ1Njc4OTAxMjM0NTY3ODkwMTI="  |
| F1–F6         | string  | -        | Код формата Ф.1–Ф.6 в кодировке Base64. <br><br>Если не указан — не используется. | "RjEtREFUQS0wMDE=" |

#### FractionalQuantity

| **Имя поля** | **Тип** | **Обяз** | **Назначение**                                                                                                                        | **Пример** |
| --- | --- | --- | --- | --- |
| Numerator    | int     | +        | Числитель. <br><br>Обязателен, если передан **FractionalQuantity**. <br><br>Если 0 или не указан — дробное количество не применяется. | 208        |
| Denominator  | int     | +        | Знаменатель. <br><br>Обязателен, если передан **FractionalQuantity**. Не должен быть 0.                                               | 1          |

#### OperationalAttribute

| **Имя поля**  | **Тип** | **Обяз** | **Назначение**                                                      | **Пример**                             |
| --- | --- | --- | --- | --- |
| DateTime      | string  | -        | Дата и время операции. <br><br>Если не указано — тег не передаётся. | "03.07.2026"                           |
| OperationId   | string  | -        | Идентификатор операции. <br><br>Если не указан — тег не передаётся. | "24f4bffe-98ef-4627-846c-b1f74c5a495b" |
| OperationData | string  | -        | Данные операции. <br><br>Если не указаны — тег не передаётся.       | "Оплата по договору №784/2026"         |

#### VendorData

| **Имя поля** | **Тип**  | **Обяз** | **Назначение** | **Пример**  |
| --- | --- | --- | --- | --- |
| Phones       | string[] | +        | Телефоны поставщика. Должен быть хотя бы один корректный российский номер (11 цифр). Сервер автоматически исправляет формат (удаляет лишние символы, заменяет 8 на 7). <br><br>Если после исправления ни одного валидного номера не осталось — ошибка "Телефон поставщика в строке чека N не некорректен". | [<br>"+79031234567", <br>"+79169876543"<br>] |
| Name         | string   | +  | Наименование поставщика. <br><br>Если пустой — ошибка "Наименование поставщика отсутствует в строке чека N".  | "ИП 'Ромашка'"  |
| Vatin        | string   | +        | ИНН поставщика. <br><br> Если пустой — ошибка "ИНН поставщика отсутствует в строке чека N". | "5262107639" |

#### TextString

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Text         | string  | -        | Текст строки. Поддерживает префиксы перед текстом: `[big]`, `[small]`, `[bold]`, `[H1]`–`[H5]`, `[left]`, `[right]`, `[center]`, `[QR]`, `[line]`, `[line,dotted]`. Префикс `[line]` печатает разделительную линию вместо текста. URL в **Text** автоматически печатается как QR-код. Если пустой — строка пропускается (кроме префикса `[line]`). | "[big, center]Добро пожаловать" |
| Font         | string  | -        | Шрифт: Normal, Bold, Small, Medium, Big, H1–H5. <br><br>Если не указан — Normal. Префикс в **Text** имеет приоритет над **Font**. | "Big"   |
| Alignment    | string  | -        | Выравнивание: left, right, center, width. <br><br>Если не указано — left. Префикс в **Text** имеет приоритет над **Alignment**. | "left"  |

#### ApiElectronicPayment

| **Имя поля**          | **Тип** | **Обяз** | **Назначение** | **Пример**  |
| --- | --- | --- | --- | --- |
| Amount                | decimal | +        | Сумма безналичной оплаты по данной транзакции. <br><br>Если не указана — передаётся 0.  | 50  |
| PaymentMethod         | int     | -        | Признак способа оплаты:<br>0 — предоплата 100%;<br>1 — предоплата;<br>2 — аванс;<br>3 — полный расчёт;<br>4 — частичный расчёт и кредит;<br>5 — передача в кредит;<br>6 — оплата кредита. <br><br>Если не указан — не передаётся. | 3 |
| Identifiers           | string  | -        | Идентификаторы безналичной оплаты (RRN, номер транзакции и т.п.). <br><br>Если не указан — тег не передаётся.  | "RRN=123456789012"         |
| AdditionalInformation | string  | -        | Доп. сведения об оплате. <br><br>Если не указаны — тег не передаётся. | "Терминал №1, карта *1234" |

#### IndustryAttribute

| **Имя поля**   | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| IdentifierFoiv | string  | -        | Идентификатор ФОИВ (тег 1262). Сервер дополняет 2-символьный код нулём слева до 3 символов ("30" станет "030").<br><br>Если не указан — тег не передаётся. | "3" |
| DocumentDate   | string  | -        | Дата документа-основания в формате "DD.MM.YYYY". <br><br>Если не указана — тег не передаётся.  | "01.07.2026"  |
| DocumentNumber | string  | -        | Номер документа-основания. <br><br>Если не указан — тег не передаётся. | "6" |
| AttributeValue | string  | -        | Идентификатор проверки кода маркировки. Для онлайн-проверки содержит параметры `UUID` и `Time`. Для офлайн-проверки дополнительно включает параметры Inst и Ver. Если значение не указано, тег не передаётся. | Онлайн: "UUID=fff5db9c-c0cb-4b1f-ac4b-8f0b92879780&Time=1746530733410". <br><br>Офлайн: "UUID=2ecabfd9-e863-46a4-aa4e-23b96508d804&Time=1784090059735&Inst=6e7ff224-0e08-41ed-844c-d386675f4e50&Ver=1ebf3971-5b24-403b-ae6d-8bb5a0383c14"|

#### AgentData

| **Имя поля**                 | **Тип**  | **Обяз** | **Назначение** | **Пример**                                  |
| --- | --- | --- | --- | --- |
| PayingAgentOperation         | string   | -        | Операция платежного агента (тег 1044). <br><br>Если не указана — в чек не попадает.| "Прием платежей"                            |
| PayingAgentPhone             | string[] | -        | Телефон платежного агента (тег 1073). <br><br>Если не указан или массив пустой — тег не передаётся. <br><br>Некорректный формат — сервер попытается исправить; при неудаче — ошибка. | ["+79021654832"] |
| ReceivePaymentsOperatorPhone | string[] | -        | Телефон оператора по приёму платежей (тег 1074). <br><br>Если не указан — тег не передаётся. <br><br>Некорректный формат — сервер попытается исправить; при неудаче — ошибка.  | ["+790216748367"]  |
| MoneyTransferOperatorPhone   | string[] | -        | Телефон оператора перевода (тег 1075). <br><br>Если не указан — тег не передаётся ( и если не заполнены другие поля группы MoneyTransferOperator).                                   | ["+790216702167"]                           |
| MoneyTransferOperatorName    | string   | -        | Наименование оператора перевода (тег 1026). <br><br>Если не указано — тег не передаётся ( и если не заполнены другие поля группы MoneyTransferOperator).                             | "Иванов И.И."                               |
| MoneyTransferOperatorAddress | string   | -        | Адрес оператора перевода (тег 1005). <br><br>Если не указан — тег не передаётся ( и если не заполнены другие поля группы MoneyTransferOperator).                                     | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| MoneyTransferOperatorVatin   | string   | -        | ИНН оператора перевода (тег 1016). <br><br>Если не указан — тег не передаётся ( и если не заполнены другие поля группы MoneyTransferOperator).                                       | "7722345678"                                |

#### UserAttribute

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример**|
| --- | --- | --- | --- | --- |
| Name         | string  | +        | Название реквизита. <br><br>Если не указано — реквизит не передаётся. | "НомерЗаказа"   |
| Value        | string  | +        | Значение реквизита. <br><br>Если не указано — реквизит не передаётся. | "ORD-2026-0042" |

#### ApiBarcode

| **Имя поля** | **Тип** | **Обяз** | **Назначение**  | **Пример**                                 |
| --- | --- | --- | --- | --- |
| Type         | string  | +        | Тип штрихкода: UPCA,<br>CODE39, <br>EAN13, <br>EAN8, <br>UPCE, <br>ITF, <br>CODABAR, <br>CODE93, <br>CODE128, <br>PDF417, <br>CODE32, <br>QR. <br><br>Если не указан — позиция пропускается. | "QR" |
| Value        | string  | -        | Значение штрихкода в текстовом виде. <br><br>Обязательно **Value** или **ValueBase64**.<br><br>Если оба пустые — позиция пропускается. | "https://roga-i-kopyta.ru"  |
| ValueBase64  | string  | -        | Значение штрихкода в кодировке Base64. | "c2VjdXJlLXJhbmRvbS12YWx1ZS0xMjM0NTY3ODkw" |
| Alignment    | string  | -        | Выравнивание штрихкода:<br>left, <br>right, <br>center.<br><br>Если не указано — center. | "center" |

#### Picture

| **Имя поля** | **Тип** | **Обяз** | **Назначение**                                                                                                      | **Пример**       |
| --- | --- | --- | --- | --- |
| Value        | string  | +        | Изображение в Base64. <br><br>Если пустой — позиция пропускается.                                                   | "iVBORw0KGgo..." |
| Alignment    | int     | -        | Выравнивание: <br>0 — слева, <br>1 — по центру (по умолчанию), <br>2 — справа. <br><br>Если не указано — 1 (центр). | 1                |

#### SeparatorLine

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| lineStyle    | int     | -        | Стиль разделительной линии: <br>0 — Solid (сплошная, по умолчанию); <br>1 — Bold (жирная); <br>2 — Dashed (штриховая); <br>3 — Dotted (пунктирная); <br>4 — Double (двойная). <br><br>Если не указан — сплошная линия. | 3 |

#### AdvItem

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| PrintLine | [PrintLine](#printline) | + | Строка печати | "PrintLine": {<br>"Type": 1,<br>"Line": "Текст",<br>"LineRight": "",<br>"Alignment": 1,<br>"Font": 5,<br>"Wrap": true<br>} |

#### PrintLine

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | int | + | Тип строки.  <br>0 — фискальная строка;  <br>1 — текстовая строка;  <br>2 — штрихкод;  <br>3 — изображение;  <br>4, Line — линия. <br><br>Если не указано — принимается значение 1 (TextString). | 1 |
| Width | int | - | Ширина. <br><br>Если не указано — 0 (ширина по содержимому). | 0 |
| Scale | int | - | Масштаб. <br><br>Если не указано — применяется масштаб 100%. | 100 |
| Line | string | - |Текст строки (левая часть) | "Текст" |
| LineRight | string | - |Текст строки (правая часть) | "Информация" |
| Alignment | int | - |Выравнивание.  <br>0 — выравнивание по левому краю;  <br>1 — Выравнивание по центру;  <br>2 — Выравнивание по правому краю;  <br>3 — По ширине. <br><br>Если не указано — по левому краю. | 1 |
| Font | int | - |Шрифт. <br>0 — Шрифт для обычных строк;  <br>1 — Жирный шрифт;  <br>2 — Мелкий шрифт;  <br>3 — Средний шрифт;  <br>4 — Крупный шрифт ;  <br>5 — Стиль заголовка первого уровня (H1);  <br>6 — Стиль заголовка второго уровня (H2);  <br>7 — Стиль заголовка третьего уровня (H3);  <br>8 — Стиль заголовка четвёртого уровня (H4);  <br>9 — Стиль заголовка пятого уровня (H5). <br><br>Если не указано — 0 (Normal). | 0 |
| Wrap | bool | - |Признак переноса строк.  <br>false - строка обрезается;  <br>true - строка переносится. Если не указано — принимается значение true. | true |
| Barcode | [Barcode](#barcode) | - | Штрихкод | "Barcode": {<br>"Type": "QR",<br>"Value": "https://www.rbsoft.ru/",<br>"PrintText": 0,<br>"Height": 30,<br>"BarWidth": 6<br>} |
| SeparatorLine | [SeparatorLine](#separatorline) | - | Разделительная линия | "SeparatorLine": {<br>"LineStyle": 0<br>} |
| Picture | [Picture](#picture) | - | Картинка | "Picture": {<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",<br>"Alignment": 2,<br>"Width": 200,<br>"Height": 80<br>} |

#### Barcode

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | string | + | Тип штрихкода.  <br>Возможные значения:  <br>UPCA,  <br>CODE39,  <br>EAN13,  <br>EAN8,  <br>UPCE,  <br>ITF,  <br>CODABAR,  <br>CODE93,  <br>CODE128,  <br>PDF417,  <br>CODE32,  <br>QR | "QR" |
| Value | string | + | Значение штрихкода | "t=20260522T1506&s=50.00&fn=9999078902010507&i=343&fp=1352614355&n=1" |
| PrintText | int | - | Задает способ печати текста штрихкода(только для одномерных штрихкодов).  <br>0 — не печатать  <br>1 — печатать снизу  <br>2 — печатать сверху  <br>3 — печатать сверху и снизу. Если не указано — 0. | 1 |
| Height | int | - | Высота штрихкода в точках.  <br>Допустимые значения свойства: 0..1199 | 100 |
| BarWidth | int | - | Ширина штриха в точках  <br>Допустимые значения свойства: 0..1199  <br>Рекомендуемое значение – 2. | 100 |

#### RequestKM

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Guid | string | - | Уникальный код запроса. Его можно описать для дальнейшего применения в запросе marking/km/confirm. | "46dc63e5-8efa-4da6-b8ee-d6188da7b26a" |
| NotSendToServer | bool | \- | Признак того, что результат проверки не нужно отправлять на сервер ОИСМ. Применяется, если при продаже маркированного товара указывать код маркировки в чеке необязательно. <br><br>Если не указано — принимается значение `false`. | false |
| WaitForResult | bool | - | Признак ожидания ответа ОИСМ. <br><br>Если не указано — принимается значение `false`. | false |
| MarkingCode | string | + | Код маркировки в кодировке Base64. | "MDEwNDY3MDU0MDE3NjA5OTIxNSdXOVVtHTkzZEdWeg==" |
| PlannedStatus | int | + | Планируемый статус (таблица 105 ФФД): <br>1 — реализован, <br>2 — мерный в стадии реализации, <br>3 — возвращён, <br>4 — часть возвращена, <br>255 — статус не изменился | 1 |
| Quantity | decimal | - | Количество товара. | 1 |
| MeasureOfQuantity | int | \- | Мера количества товара (таблица 114 ФФД). <br><br>Если не указано — 0. | 0 |
| FractionalQuantityNumerator | int | \- | Числитель дробного количества товара. Указывается вместе с **FractionalQuantityDenominator**. | 0 |
| FractionalQuantityDenominator | int | \- | Знаменатель дробного количества товара. | 0 |

---

### Выходные типы данных

#### Barcode

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | string | - | Тип штрихкода.  <br>Возможные значения:  <br>UPCA,  <br>CODE39,  <br>EAN13,  <br>EAN8,  <br>UPCE,  <br>ITF,  <br>CODABAR,  <br>CODE93,  <br>CODE128,  <br>PDF417,  <br>CODE32,  <br>QR | "QR" |
| Value | string | - | Значение штрихкода | "t=20260522T1506&s=50.00&fn=9999078902010507&i=343&fp=1352614355&n=1" |
| PictureBase64 | string | - | Изображение штрихкода, закодированное в Base64 | "iVBORw0KGgoAAAANSUhEUgAAAH0AAAB9CAIAAAAA4vtyAAAA..." |
| PrintText | int | - | Задает способ печати текста штрихкода(только для одномерных штрихкодов).  <br>0 — не печатать  <br>1 — печатать снизу  <br>2 — печатать сверху  <br>3 — печатать сверху и снизу | 1 |
| Height | int | - | Высота штрихкода в точках.  <br>Допустимые значения свойства: 0..1199 | 100 |
| BarWidth | int | - | Ширина штриха в точках. <br>Допустимые значения свойства: 0..1199  <br>Рекомендуемое значение – 2. | 100 |

#### SeparatorLine

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| LineStyle | int | - | Стиль разделительной линии: <br>0 — сплошная линия; <br>1 — жирная линия;  <br>2 — штриховая линия;  <br>3 — пунктирная линия;  <br>4 — двойная линия | 1 |

#### FnInfo

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Execution | string | - | Исполнение ФН | "" |
| FnContainsKeysUpdaterServerUri | bool | - | ФН содержит URI сервера ОКП | false |
| FiscalizationsCount | int | - | Количество проведенных регистраций | 0 |
| FiscalizationsFree | int | - | Количество оставшихся регистраций | 0 |
| FiscalizationDocumentNumber | string | - | Номер документа регистрация фискального накопителя | "0" |
| FiscalizationDateTime | datetime | - | Дата и время операции регистрации фискального накопителя | "2026-02-15T00:00:00" |
| ReasonCode | int | - | Код причины перерегистрации: 0 — Нет причины / первичная регистрация (не перерегистрация); <br>1 — Замена ФН; <br>2 — Замена ОФД; <br>3 — Изменение реквизитов; <br>4 — Изменение настроек ККТ | 0 |
| LivePhase | string | - | Фаза жизни ФН: <br>init - настройка ФН; <br>configured - настроен, готов в активации; <br>fiscalMode - фискальный режим; <br>postFiscalMode - постфискальный режим; <br>accessArchive - доступ к архиву ФН; <br>unknown - неизвестная фаза жизни | "fiscalMode" |
| Version | string | - | Версия ФН | "1.0" |
| RnNumber | string | - | Регистрационный номер ККТ | "00031415926" |
| FnsUrl | string | - | Адрес сайта уполномоченного органа(ФНС) в сети «Интернет» | "nalog.ru" |
| SenderEmail | string | - | Адрес электронной почты отправителя чека | "ivanov@mail.ru" |
| FfdVersion | string | - | Версия ФФД ФН | "1.2" |
| SerialNumber | string | - | Серийный номер ФН | "0123123123123" |
| OrganizationName | string | - | Название организации | "ООО 'Ромашка'" |
| Vatin | string | - | ИНН организации | "7722345678" |
| ValidityDate | datetime | - | Срок действия ФН | "2027-06-08T17:22:38.2396579+08:00" |
| SaleAddress | string | - | Адрес установки ККТ для проведения расчетов | "г.Улан-Удэ, ул.Виноградная, д11А, офис 25" |
| SaleLocation | string | - | Место проведения расчетов | "Офис" |
| TaxVariant | int | - | Коды систем налогообложения: <br>1 — Общая (ОСН);<br> 2 — Упрощенная Доход(УСН);<br> 3 — Упрощенная Доход минус Расход(УСНД_Р);<br> 4 — Единый налог на вмененный доход(ЕНВД);<br> 5 — Единый сельскохозяйственный налог(ЕСН);<br> 6 — Патентная система налогообложения(ПСН)  | 63 |
| SignOfAgent | int | - | Коды признаков агента. <br> 0 — Банковский платежный агент;<br> 1 — Банковский платежный субагент;<br> 2 — Платежный агент;<br> 3 — Платежный субагент;<br> 4— Поверенный;<br> 5 — Комиссионер;<br> 6 — Агент;<br> | 127 |

#### Ofd

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | - | Имя ОФД | "Тестовый ОФД" |
| Vatin | string | - | ИНН ОФД | "1234554321" |
| Host | string | - | Адрес сервера ОФД | "ofd.example.ru" |
| Port | int | - | Порт сервера ОФД | 7777 |

#### Warnings / FnWarnings

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| CriticalError | bool | - | Критическая ошибка ФН | false |
| MemoryOverflow | bool | - | Память ФН переполнена | false |
| NeedReplacement | bool | - | Требуется срочная замена ФН | false |
| OfdTimeout | bool | - | Превышено время ожидания ответа от ОФД | false |
| ResourceExhausted | bool | - | Исчерпан ресурс ФН | false |

#### FnModes

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| PrinterAutomatic | bool | - | Признак установки принтера в автомате | false |
| OfflineMode | bool | - | Признак автономного режима | false |
| ServiceSign | bool | - | Признак расчетов за услуги | false |
| BsoSign | bool | - | Признак формирования только БСО | false |
| CalcOnlineSign | bool | - | Признак ККТ для расчетов только в Интернет | false |
| DataEncryption | bool | - | Признак шифрование данных | false |
| SaleExcisableGoods | bool | - | Продажа подакцизного товара | false |
| SignOfGambling | bool | - | Признак проведения азартных игр | false |
| SignOfLottery | bool | - | Признак проведения лотереи | false |
| Pawnshop | bool | - | Признак применения при осуществлении ломбардами кредитования граждан | false |
| Assurance | bool | - | Признак применения при осуществлении деятельности по страхованию | false |
| Marking | bool | - | Признак применения при осуществлении торговли товарами, подлежащими обязательной маркировке средствами идентификации | false |
| VendingMachine | bool | - | Признак применения в автоматическом торговом автомате | false |
| CateringServices | bool | - | Признак применения при оказании услуг общественного питания | false |
| WholesaleTrade | bool | - | Признак применения о оптовой торговле с организациями и ИП | false |
| AutomaticMode | bool | - | Признак автоматического режима | false |

#### KktInfo

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| FfdVersion | string | - | Версия ФФД | "1.2 |
| FnFfdVersion | string | - | Версия ФФД ФН | "1.2" |
| TimeZone | int | - | Часовая зона: <br>0 — Авто; <br>1 — 1 часовая зона (МСК-1 / UTC+2); <br>2 — 2 часовая зона (МСК / UTC+3); <br>3 — 3 часовая зона (МСК+1 / UTC+4); <br>4 — 4 часовая зона (МСК+2 / UTC+5); <br>5 — 5 часовая зона (МСК+3 / UTC+6); <br>6 — 6 часовая зона (МСК+4 / UTC+7); <br>7 — 7 часовая зона (МСК+5 / UTC+8); <br>8 — 8 часовая зона (МСК+6 / UTC+9); <br>9 — 9 часовая зона (МСК+7 / UTC+10); <br>10 — 10 часовая зона (МСК+8 / UTC+11); <br>11 — 11 часовая зона (МСК+9 / UTC+12) | 7 |
| KktLicenses | [KktLicenseInfo](#kktlicenseinfo)[] | - | Массив лицензий ККТ | \[\] |
| IsFiscal | bool | - | Фискальный режим | true |
| LineLength | int | - | Ширина чековой ленты | 64 |
| LineLengthPixels | int | - | Ширина чековой ленты в пикселях | 0 |
| DeviceClass | int | - | Тип устройства: <br>1 — Принтер; <br>2 — Чековый принтер;  <br>3 — Фискальный регистратор (не онлайн-ккм)  <br>4 — Онлайн-ккм, применяемая в РФ в соответствии с ФЗ-54.  <br>5 — Эквайринговый терминал; <br>6 — Терминал сбора данных; <br>7 — Электронные весы; <br>8 — Электронные весы с печатью этикеток; <br>9 — Сканер штрихкодов | 7 |
| SerialNumber | string | - | Заводской номер ККТ | "0020260207" |
| FirmwareVersion | string | - | Версия прошивки | "2026" |
| ConfigurationVersion | string | - | Версия конфигурации прошивки устройства | "02.07" |

#### DriverInfo

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | string | - | Тип драйвера | "Atol" |
| Version | string | - | Версия драйвера | "26.01.27" |
| Vendor | string | - | Данные поставщика | "Atol" |

#### KktStatus

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| IsFnPresent | bool | - | Присутствует ли фискальный накопитель | false |
| IsFnError | bool | - | Находится ли фискальный накопитель в состоянии ошибки | false |
| IsIsmDisconnected | bool | - | Доступен ли информационной системы маркировки | false |
| IsOfdDisconnected | bool | - | Доступен ли оператор информационной системы маркировки | false |
| Backlog | [Backlog](#backlog) | - | Данные о непереданных документах | "Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>} |
| Ism | [ExchangeStatusIsm](#exchangestatusism) | - | Состояние обмена с ИСМ | "ExchangeStatusIsm": {<br>"AddressIsm": "testism.ru",<br>"PortIsm": 20,<br>"Errors": {<br>"FnCommandCode": 0,<br>"DocumentNumber": 0,<br>"LastSuccessConnectionDateTime": "0001-01-01T00:00:00",<br>"Fn": {<br>"Code": 0<br>},<br>"Network": {<br>"Code": 0<br>},<br>"Ism": {<br>"Code": 0<br>}<br>},<br>"Status": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>},<br>"Warnings": {<br>"DataForSendIsEmpty": false<br>}<br>} |
| Warnings | [Warnings](#warnings--fnwarnings) | - | Предупреждения ФН | "Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>} |
| ShiftNumber | int | - | Номер смены | 17 |
| DocNumber | int | - | Номер фискального документа | 98 |
| DocNumberInShift | int | - | Номер фискального документа за смену | 3 |
| CashSum | decimal | - | Сумма наличных в денежном ящике | 2128.0 |
| TotalSum | decimal | - | Сумма выручки | 0.0 |
| IsFiscal | bool | - | Фискальный | true |
| OpenShiftTime | DateTime | - | Время открытия кассовой смены | "0001-01-01T00:00:00" |
| IsShiftOpened | bool | - | Смена открыта | false |
| IsShiftExpired | bool | - | Смена истекла | false |
| ComputerTime | DateTime | - | Время получения данных | "2026-05-23T19:50:04.7531056+08:00" |
| DeviceTime | DateTime | - | Время в часах устройства | "2026-05-23T19:50:04.7531056+08:00" |
| IsDrawerOpened | bool | - | Открыт денежный ящик | false |
| IsCheckPaperPresent | bool | - | Наличие чековой ленты | false |
| IsControlPaperPresent | bool | - | Наличие контрольной ленты | false |
| IsWaitContinuePrint | bool | - | Ожидание продолжения печати | false |
| IsCoverOpened | bool | - | Открыта ли крышка | false |
| IsBatteryLow | bool | - | Аккумулятор разряжен | false |
| IsOpenDocument | bool | - | Открытый документ | false |
| LineLength | int | - | Ширина чековой ленты | 42 |
| LineLengthPixels | int | - | Ширина чековой ленты в пикселях | 512 |
| TaskId | string | - | Идентификатор текущей задачи | "00000000-0000-0000-0000-000000000000" |
| Error | int | - | Код ошибки | 0 |
| ErrorCodeDescription | string | - | Описание ошибки устройства по данным драйвера | "OK" |
| DriverMode | int | - | Режим по данным драйвера | 0 |
| DriverModeDescription | string | - | Описание режима Mode по данным драйвера | "" |
| DriverAdvancedMode | int | - | Специальный режим по данным драйвера | 0 |
| DriverAdvancedModeDescription | string | - | Описание режима AdvancedMode по данным драйвера | 0 |
| LicenseStatus | int | - | Статус состояния лицензии | 0 |
| License | [License](#license) | - | Описание лицензии | "License": {<br>"code": 0,<br>"isEndUser": false,<br>"isActivated": false,<br>"isBlocked": false,<br>"blockDate": "0001-01-01T00:00:00",<br>"date": "0001-01-01T00:00:00",<br>"expired": "0001-01-01T00:00:00",<br>"updateExpired": "0001-01-01T00:00:00",<br>"limitInstalls": 0,<br>"needObjectActivation": false,<br>"limitObjects": 0,<br>"setupTokenIndex": 0,<br>"licenseUpdated": "0001-01-01T00:00:00"<br>} |
| LicenseUpdated | DateTime | - | Время последней проверке лицензии | "0001-01-01T00:00:00" |

#### ExchangeStatusIsm

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Address       | string   | - | Адрес сервера информационной системы маркировки        | "ism.example.ru" |
| Port          | int      | - | Порт сервера информационной системы маркировки         | 8888        |
| Errors        | [ErrorsIsm](#errorsism) | - | Сведения об ошибках обмена данными  | "Errors": {<br>"FnCommandCode": 0,<br>"DocumentNumber": 0,<br>"LastSuccessConnectionDateTime": "0001-01-01T00:00:00",<br>"Fn": {<br>"Code": 0<br>},<br>"Network": {<br>"Code": 0<br>},<br>"Ism": {<br>"Code": 0,<br>"Description": ""<br>}<br>} |
| Backlog       | [Backlog](#backlog)  | - | Сведения о непереданных документах  | "Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>}     |

#### ErrorsIsm

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| FnCommandCode | int      | - | Код команды фискального накопителя            | 0            |
| DocumentNumber   | int      | - | Номер документа                               | 0            |
| LastSuccessConnectionDateTime       | DateTime | - | Время последнего успешного подключения        | "0001-01-01T00:00:00" |
| Fn | [FnErrors](#fnerrors) | - | Код ошибки фискального накопителя      |       "Fn": {<br>"Code": 0<br>},      |
| Network | [NetworkErrors](#networkerrors) | - | Код ошибки сети                    | "Network": {<br>"Code": 0<br>}        |
| Ism | [IsmErrors](#ismerrors) | - | Код ошибки информационной системы маркировки |   "Ism": {<br>"Code": 0,<br>"Description": ""<br>}       |

#### FnErrors

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Code         | int     | - | Код ошибки ФН | 0 |

#### NetworkErrors

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Code         | int     | - | Код ошибки сети   | 0 |

#### IsmErrors

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Code         | int     | - | Код ошибки ИСМ     | 0 |
| Description | string | - | Описание ошибки | "Нет ошибок" |

#### Backlog

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DocumentsCounter | int | - | Количество непереданных документов | 0 |
| DocumentFirstNumber | int | - | Номер первого непереданного документ | 0 |
| DocumentFirstDateTime | DateTime | - | Дата и время первого из непереданных документов | "0001-01-01T00:00:00" |

#### License

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| code | int | - | Код ошибки | 0 |
| isEndUser | bool | - | Признак лицензии на конечного пользователя.  <br>true — лицензия выдана конечному пользователю.  <br>false — лицензия выдана партнеру для перепродажи | false |
| isActivated | bool | - | Признак активации | false |
| isBlocked | bool | - | Признак блокировки | 0 |
| blockDate | DateTime | - | Дата блокировки | "0001-01-01T00:00:00" |
| date | DateTime | - | Дата продажи | "0001-01-01T00:00:00" |
| expired | DateTime | - | Дата истечения срока действия | "0001-01-01T00:00:00" |
| updateExpired | DateTime | - | Дата истечения доступа к обновлениям | "0001-01-01T00:00:00" |
| limitInstalls | int | - | Количество разрешенных привязок равно числу ККМ, которые можно привязать к одной лицензии | 0 |
| needObjectActivation | bool | - | Признак необходимости привязки объектов лицензирования после привязки. | 0 |
| limitObjects | int | - | Количество объектов привязки на каждую установку | 0 |
| setupTokenIndex | int | - | Индекс установочного токена | 0 |
| LicenseUpdated | DateTime | - | Время последней проверке лицензии | "0001-01-01T00:00:00" |

#### CashDrawer 

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Sum | decimal | - | Сумма | 1365.68 |
| Count | int | - | Количество | 7 |

#### ShiftIncome

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Count | int | - | Количество | 0 |
| Sum | decimal | - | Сумма | 0.0 |

#### ShiftCounters 

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| SumCorrection | decimal | - | Сумма коррекций | 0.0 |
| NumberCorrections | int | - | Количество коррекций | 0 |
| Sales | [DocData](#docdata) | - | Чеки прихода | "Sales": {<br>"Count": 0,<br>"Sum": 50,<br>"Payments": {<br>"Sum": 24739.54,<br>"Cash": 25163.08,<br>"Credit": 9400.00,<br>"Electronically": 4949.54,<br>"Barter": 2350.00,<br>"Prepaid": 4700.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_20": 93.03,<br>"TaxVat_7": 1536.90<br>}<br>} |
| SalesReturn | [DocData](#docdata) | Чеки возврата прихода | "SalesReturn": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| SalesCorrection | [DocData](#docdata) | - | Чеки прихода | "SalesCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| SalesReturnCorrection | [DocData](#docdata) | - | Чеки возврата прихода | "SalesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| Purchases | [DocData](#docdata) | - | Чеки расхода | "Purchases": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| PurchasesReturn | [DocData](#docdata) | - | Чеки возврата расхода | "PurchasesReturn": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| PurchasesCorrection | [DocData](#docdata) | - | Чеки расхода | "PurchasesCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>}, |
| PurchasesReturnCorrection | [DocData](#docdata) | - | Чеки возврата расхода | "PurchasesReturnCorrection": {<br>"Count": 45,<br>"Sum": 4500.0,<br>"Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>},<br>"Tax": {<br>"TaxVat_7": 294.30<br>}<br>} |

#### DocData

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Count | int | - | Количество | 0 |
| Sum | decimal | - | Сумма | 50.0 |
| Payments | [DocDataPayments](#docdatapayments) | - | Оплаты | "Payments": {<br>"Sum": 4500.0,<br>"Cash": 4500.00,<br>"Credit": 1800.00,<br>"Electronically": 900.00,<br>"Barter": 450.00,<br>"Prepaid": 900.00<br>}, |
| Discount | [RegData](#regdata) | - | Скидка | Discount": {<br>"Count": 0,<br>"Sum": 0.0<br>} |
| Adding | [RegData](#regdata) | - | Надбавки | "Adding": {<br>"Count": 0,<br>"Sum": 0.0<br>} |
| Tax | [Taxes](#taxes) | - | Налоги | "Tax": {<br>"TaxVat_7": 294.30<br>} |

#### DocDataPayments

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Sum | decimal | - | Общая сумма чеков | 50.0 |
| Cash | decimal | - | Сумма оплат наличными | 50.0 |

#### RegData

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Count | int | - | Количество | 0 |
| Sum | decimal | - | Сумма | 0.0 |

#### Taxes

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| TaxVat_10 | decimal | - | Ставка НДС 10% | 0 |
| TaxVat_110 | decimal | - | Ставка 10/110 | 0 |
| TaxVat_0 | decimal | - | Ставка НДС 0 | 0 |
| TaxVat_NO | decimal | - | Ставка без НДС | 0 |
| TaxVat_20 | decimal | - | Ставка НДС 20% | 0 |
| TaxVat_120 | decimal | - | Ставка 20/120 | 0 |
| TaxVat_22 | decimal | - | Ставка НДС 22% | 0 |
| TaxVat_122 | decimal | - | Ставка 22/122 | 0 |
| TaxVat_5 | decimal | - | Ставка НДС 5% | 0 |
| TaxVat_105 | decimal | - | Ставка НДС 5/105 | 0 |
| TaxVat_7 | decimal | - | Ставка НДС 7% | 0 |
| TaxVat_107 | decimal | - | Ставка НДС 7/107 | 0 |

#### OutputParametersV4

| **Имя поля** | **Тип** | **Обяз** |**Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| NumberOfChecks | int | - | Количество кассовых чеков за смену | 1 |
| NumberOfDocuments | int | - | Количество общее ФД за смену | 1 |
| ResourcesFn | int | - | Срок действия ключей фискального признака | 365 |
| ShiftNumber | int | - | Номер открытой смены/Номер закрытой смены | 2 |
| CheckNumber | int | - | Номер последнего фискального документа | 3 |
| ShiftClosingCheckNumber | int | - | Номер последнего чека за смену | 1 |
| DateTime | DateTime | - | Дата и время формирования фискального документа | "2026-05-23T23:45:15.6391315+08:00" |
| ShiftState | int | - | Состояние смены:  <br>1 — Закрыта;  <br>2 - Открыта;  <br>3 — Истекла | 2 |
| CashBalance | decimal | - | Остаток наличных денежных средств в кассе | 0.0 |
| FnValidityDate | DateTime | - | Срок действия ФН | "2027-05-23T23:45:15.6391315+08:00" |
| Backlog | [Backlog](#backlog) | - | Данные о непереданных документах | "Backlog": {<br>"DocumentsCounter": 0,<br>"DocumentFirstNumber": 0,<br>"DocumentFirstDateTime": "0001-01-01T00:00:00"<br>} |
| FnWarnings | FnWarnings | Флаги состояния ФН | "FnWarnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>} |

#### DeviceInfo

| **Имя поля** | **Тип** | **Обяз** |**Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| FfdVersion | string | - | Версия ФФД | "1.2" |
| FnFfdVersion | string | - | Версия ФФД ФН | "1.2" |
| TimeZone | int | - | Часовая зона: <br>0 — Авто; <br>1 — 1 часовая зона (МСК-1 / UTC+2); <br>2 — 2 часовая зона (МСК / UTC+3); <br>3 — 3 часовая зона (МСК+1 / UTC+4); <br>4 — 4 часовая зона (МСК+2 / UTC+5); <br>5 — 5 часовая зона (МСК+3 / UTC+6); <br>6 — 6 часовая зона (МСК+4 / UTC+7); <br>7 — 7 часовая зона (МСК+5 / UTC+8); <br>8 — 8 часовая зона (МСК+6 / UTC+9); <br>9 — 9 часовая зона (МСК+7 / UTC+10); <br>10 — 10 часовая зона (МСК+8 / UTC+11); <br>11 — 11 часовая зона (МСК+9 / UTC+12) | 7 |
| KktLicenses | [KktLicenseInfo](#kktlicenseinfo)[] | - | Массив лицензий ККТ | \[\] |
| IsFiscal | bool | - | Фискальный режим | true |
| LineLength | int | - | Ширина чековой ленты | 64 |
| LineLengthPixels | int | - | Ширина чековой ленты в пикселях | 0 |
| DeviceClass | int | - | Тип устройства: <br>1 — Принтер; <br>2 — Чековый принтер;  <br>3 — Фискальный регистратор (не онлайн-ккм); <br>4 — Онлайн-ккм, применяемая в РФ в соответствии с ФЗ-54; <br>5 — Эквайринговый терминал  <br>6 — Терминал сбора данных; <br>7 — Электронные весы; <br>8 — Электронные весы с печатью этикеток; <br>9 — Сканер штрихкодов | 7 |
| Model | string | - | Название модели | АТОЛ FPrint-22ПТК |
| SerialNumber | string | - | Заводской номер ККТ | "0020260207" |
| FirmwareVersion | string | - | Версия прошивки | "2026" |
| ConfigurationVersion | string | - | Версия конфигурации прошивки устройства | "02.07" |

#### KktLicenseInfo

| **Имя поля** | **Тип** | **Обяз** |**Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Number | int | - | Номер лицензии | "1" |
| Name | string | - | Наименование лицензии | "Лицензия №1" |
| ValidFrom | DateTime | - | Дата начала действия лицензии | "2026-06-17T06:23:45" |
| ValidUntil | DateTime | - | Дата окончания действия лицензии | "2027-06-17T06:23:45" |
| UnitVersion | string | - | Максимально поддерживаемая конфигурация. | "5.14" |
| Description | string | - | Описание последствий окончания лицензии | "Лицензия истекла!" |
| IsActive | bool | - | Лицензия действует для текущей прошивки | "true" |

#### OutputParameters

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DepartmentTotals | DepartmentTotals[] | - | Показатели отдела за смену | \[\] |
| NumberOfChecks | int | - | Количество кассовых чеков за смену | 1 |
| NumberOfDocuments | int | - | Количество общее ФД за смену | 1 |
| Backlog | [Backlog](#backlog) | - | Данные о непереданных документах | "Backlog": {<br>"DocumentsCounter": 1,<br>"DocumentFirstNumber": 88,<br>"DocumentFirstDateTime": "2026-05-12T18:39:00+08:00"<br>}, |
| Warnings | [Warnings](#warnings--fnwarnings) | - | Предупреждения ФН | "Warnings": {<br>"CriticalError": false,<br>"MemoryOverflow": false,<br>"NeedReplacement": false,<br>"OfdTimeout": false,<br>"ResourceExhausted": false<br>} |
| ResourcesFn | int | - | Срок действия ключей фискального признака | 365 |
| ShiftNumber | int | - | Номер открытой смены/Номер закрытой смены | 2 |
| CheckNumber | int | - | Номер последнего фискального документа | 3 |
| ShiftClosingCheckNumber | int | - | Номер последнего чека за смену | 1 |
| DateTime | DateTime | - | Дата и время формирования фискального документа | "2026-05-23T23:45:15.6391315+08:00" |
| ShiftState | int | - | Состояние смены:  <br>1 — Закрыта;  <br>2 - Открыта;  <br>3 — Истекла | 2 |
| CashBalance | decimal | - | Остаток наличных денежных средств в кассе | 0.0 |
| FnValidityDate | decimal | - | Срок действия ФН | "2027-05-23T23:45:15.6391315+08:00" |
| DocumentsCounter | long | - | Количество непереданных документов | 0 |

#### DocumentHeader

| **Имя параметра** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| OrganizationInfo | string | - | Название организации | "ООО 'Ромашка'" |
| SerialNumber | string | - | Заводской номер ККТ | "00106305393630" |
| Vatin | string | - | ИНН Организации | "7722345678" |
| Cashier | string | - | Кассир | "Иванов А.И." |
| RnNumber | string | - | Регистрационный номер ККТ | "0000000002005725" |
| Fn | string | - | Фискальный накопитель | "0123123123123" |
| FnsUrl | string | - | Адрес сайта уполномоченного органа (ФНС) в сети «Интернет» | "nalog.ru" |
| ShiftNumber | int | - | Номер смены | 39 |
| DocNumber | int | - | Номер фискального документа | 343 |
| FiscalSign | string | - | Фискальный признак документа | "1352614355" |
| OfdOrganizationName | string | - | Наименование провайдера ОФД | Тестовый ОФД |
| OfdVatin | string | - | ИНН провайдера ОФД | 7709364346 |

#### ShiftTotal

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| IsCountersReaded | bool | - | Прочитаны ли дополнительные счетчики ККМ | true |
| ShiftNumber | double | + | Номер смены | 3.0 |
| CashDrawer | [CashDrawer](#cashdrawer) | - | Детали ленежного ящиика | "CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>}, |
| ShiftIncome | [ShiftIncome](#shiftincome)  | - | Внесение | "ShiftIncome": {<br>"Count": 0,<br>"Sum": 0<br>}, |
| ShiftOutcome | [ShiftIncome](#shiftincome) | - | Выемка | "ShiftOutcome": {<br>"Count": 0,<br>"Sum": 0<br>}, |
| Counters | [ShiftCounters](#shiftcounters) | - | Счётчики за кассовую смену | "Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0<br>} |

#### ShiftCounters

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| SumCorrection | decimal | - | Сумма коррекций | 0.0 |
| NumberCorrections | int | - | Количество коррекций | 0 |

#### overallTotals

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| DataLoaded | bool | - | Все ли данные успешно прочитаны из устройства | true |
| Sum | decimal | - | Сумма | 1365.68 |
| Count | int | - | Количество | 0 |
| Counters | [ShiftCounters](#shiftcounters) | - | Чеки | "Counters": {<br>"SumCorrection": 0,<br>"NumberCorrections": 0,<br>"Sales": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"SalesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"Purchases": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturn": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>},<br>"PurchasesReturnCorrection": {<br>"Count": 0,<br>"Sum": 0,<br>"Payments": {<br>"Sum": 0<br>},<br>"Discount": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Adding": {<br>"Count": 0,<br>"Sum": 0<br>},<br>"Tax": {}<br>}<br>},<br>"CashDrawer": {<br>"Sum": 0,<br>"Count": 0<br>} |

#### CheckItem

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Name | string | - | Название | Бутылка с водой 1л." |
| Quantity | decimal | - | Количество товара | 1 |
| Price | decimal | - | Цена позиции | 50.0 |
| Department | int | - | Отдел | 0 |
| Sum | decimal | - | Сумма с учетом скидки | 50.0 |
| IsFiscal | bool | - | Фискальный режим | true |
| TaxValue | int | - | Ставка НДС | 20 |
| PaymentMode | int | - | Признак способа расчёта:<br> 0 — Не Применяется, <br>1 — Полная предварительная оплата до момента передачи предмета расчета, <br>2 — Частичная предварительная оплата до момента передачи предмета расчета, <br>3 — Аванс, <br>4 — Полная оплата, в том числе с учетом аванса (предварительной оплаты) в момент передачи предмета расчета, <br>5 — Частичная оплата предмета расчета в момент его передачи с последующей оплатой в кредит, <br>6 — Передача предмета расчета без его оплаты в момент его передачи с последующей оплатой в кредит, <br>7 — Оплата предмета расчета после его передачи с оплатой в кредит (оплата кредита) | 3 |
| ItemType | int | - | Признак предмета расчёта(тег 1030) | 10 |
| ExciseAmount | decimal | - | Сумма акциза с учетом копеек, включенная в стоимость предмета расчета | 0.0 |
| MeasureOfQuantity | int | - | Мера количества предмета расчета | 20 |

#### CustomerDetail

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Info | string | - | Наименование организации или фамилия, имя, отчество (при наличии) | "ООО 'Рога и Копыта'" |
| Inn | string | - | ИНН организации или покупателя (клиента) | "500100732259" |
| Email | string | - | Электронная почта | "kuznicov@mail.ru" |

#### QrCheckData

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Date | DateTime | - | Дата создания документа | "2026-05-24T00:11:23.5221437+08:00" |
| Amount | decimal | - | Сумма чека | 50.0 |
| Fn | string | - | Фискальный накопитель | "1234554321" |
| Fd | int | - | Фискальный документ | 370 |
| Fp | string | - | Фискальный признак | "2928907410" |
| Fd | int | - | Фискальный документ | 370 |
| N | int | - | Тип операции: <br>1 - Приход;  <br>2 - Возврат прихода;  <br>4 - Расход;  <br>5 - Возврат расхода;  <br>7 - Коррекция прихода;  <br>9 - Коррекция расхода | 370 |

#### Payments

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Cash | decimal | - | Сумма наличной оплаты | 0.0 |
| Barter | decimal | - | Сумма встречным предоставлением) | 10.0 |
| PrePaid | decimal | - | Сумма предоплатой (зачетом аванса) | 0.0 |
| Credit | decimal | - | Сумма постоплатой (в кредит) | 0.0 |
| Electronic | decimal | - | Сумма безналичными средствами | 0.0 |

#### PrintLine

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | int | - | Тип строки.  <br>0 — фискальная строка;  <br>1 — текстовая строка;  <br>2 — штрихкод;  <br>3 — изображение;  <br>4 — разделительная линия | 1 |
| Width | int | - | Ширина | 0 |
| Scale | int | - | Масштаб | 100 |
| Line | string | - | Текст строки (левая часть) | "Кассовый чек" |
| LineRight | string | - | Текст строки (правая часть) | "" |
| Alignment | int | - | Выравнивание.  <br>0 — выравнивание по левому краю;  <br>1 — Выравнивание по центру;  <br>2 — Выравнивание по правому краю;  <br>3 — По ширине | 1 |
| Font | int | - | Шрифт. <br>Normal — Шрифт для обычных строк;  <br>Bold — Жирный шрифт;  <br>Small — Мелкий шрифт;  <br>Medium — Средний шрифт;  <br>Big — Крупный шрифт;  <br>H1 — Стиль заголовка первого уровня (название документа);  <br>H2 — Стиль заголовка второго уровня (раздел документа);  <br>H3 — Стиль заголовка третьего уровня (подраздел);  <br>H4 — Стиль заголовка четвёртого уровня (детализация подраздела);  <br>H5 — Стиль заголовка пятого уровня (дополнительная детализация); | 0 |
| Wrap | bool | - | Признак переноса строк.  <br>false - строка обрезается;  <br>true - строка переносится | true |
| IsFontSpecified | bool | - | Признак, что шрифт задан явно во входящих данных или при создании строки| true |
| SeparatorLine | [SeparatorLine](#separatorline) | - | Разделительная линия | "SeparatorLine": { <br>"lineStyle": 3 <br> |
| Barcode | [Barcode](#barcode) | - | Штрихкод | "Barcode": { <br>"Type": "QR", <br>"Value": "t=20260524T1252&s=50.00&fn=9999078902010507&i=370&fp=2928907410&n=1", <br>"PictureBase64": "iVBORw0KGgoAA...", <br>"PrintText": 1, <br>"Height": 100, <br>"BarWidth": 100 <br>}, |
| Picture | [Picture](#picture) | - | Картинка | "Picture": {<br>"PictureBase64": "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==",<br>"Alignment": 2,<br>"Width": 200,<br>"Height": 80<br>} |
| IsCreateFromTemplate | bool | - | Создано из шаблона.  <br>true - создано из печатного шаблона;  <br>false — не создан из печатного шаблона | false |
| BarcodeLines | Список строк | - | Список строк, выводимые спрва или слева от штрихакодв | "ЗН ККТ: 00106305393630",  <br>"РН ККТ: 0000000002005725",  <br>"ИНН 0326031413",  <br>"ФН: 9999078902010507",  <br>"ФД: 343",  <br>"ФП: 1352614355",  <br>"ПРИХОД",  <br>"22.05.26 15:06",  <br>"Сайт ФНС:  <br>[http://www.nalog.gov.ru](http://www.nalog.gov.ru) |

#### CorrectionData

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Type | int | - | Тип коррекции: <br>0 — самостоятельно;  <br>1 — по предписанию | 1 |
| Description | string | - | Описание коррекции | "Основание" |
| Date | DateTime | - | Дата совершения корректируемого расчета | "2026-05-24T00:11:23.5221437+08:00" |
| Number | string | - | Номер предписания налогового органа | "0" |

#### ApiPayments

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Cash | decimal | - | Сумма наличной оплаты | 0.0 |
| Barter | decimal | - | Сумма встречным предоставлением) | 10.0 |
| PrePaid | decimal | - | Сумма предоплатой (зачетом аванса) | 0.0 |
| Credit | decimal | - | Сумма постоплатой (в кредит) | 0.0 |
| Electronic | decimal | - | Сумма безналичными средствами | 0.0 |

#### Picture 

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| PictureBase64 | string | + | Изображение закодированное в Base64 | "iVBORw0KGgoAAAANSUhEUgA..." |
| Alignment | int | - | Выравнивание: <br>1 - по левому краю;  <br>2 - по центру;  <br>3 - по правому краю. | 2 |
| Width | int | - | Ширина изображения при печати, в точках | 200 |
| Height | int | - | Высота изображения при печати, в точках | 80 |
| StartLineNumber | int | - | Номер первой строки печати изображения | 0 |
| EndLineNumber | int | - | Номер последней строки печати изображения | 0 |
| IsUploaded | bool | - | Признак загрузки изображения в память ККТ | false |
| Override | bool | - | Признак перезаписи изображения в памяти ККТ | false |

#### DocumentHistory

| **Имя поля** | **Тип** | **Обяз** | **Назначение** | **Пример** |
| --- | --- | --- | --- | --- |
| Time | datetime | - | Время события | "2026-05-21T16:57:25" |
| State | int | - | Код состояния задачи на момент события. | 4 |
| Description | string | - | Описание события | "Добавление в очередь" |
| Info | string | - | Дополнительная информация | "" |

#### CodeMarkInfo

| **Имя поля** | **Тип** | **Назначение** | **Пример** |
| --- | --- | --- | --- |
| cis                | string    | - | Полный код маркировки | "0104670540176099215'W9Um93dGVz" |
| valid              | bool      | - | Результат проверки валидности структуры кода идентификации (КИ) / кода маркировки (КиЗ). <br>`true` — структура валидна, <br>`false` — не валидна. | true |
| printView | string | - | Код маркировки без крипто-подписи | "0104670540176099215'W9Um" |
| groupIds | int[] | - | Массив идентификаторов товарных групп. <br>1 (lp) — Предметы одежды, бельё постельное, столовое, туалетное и кухонное <br>2 (shoes) — Обувные товары <br>3 (tobacco) — Табачная продукция <br>4 (perfumery) — Духи и туалетная вода <br>5 (tires) — Шины и покрышки пневматические резиновые новые <br>6 (electronics) — Фотокамеры (кроме кинокамер), фотовспышки и лампы-вспышки <br> 8 (milk) — Молочная продукция <br> 9 (bicycle) — Велосипеды и велосипедные рамы <br> 10 (wheelchairs) — Медицинские изделия <br>12 (otp) — Альтернативная табачная продукция<br> 13 (water) — Упакованная вода <br>14 (furs) — Товары из натурального меха <br>15 (beer) — Пиво, напитки, изготавливаемые на основе пива, слабоалкогольные напитки <br> 16 (ncp) — никотиносодержащая продукция <br>17 (bio) — Биологически активные добавки к пище <br>19 (antiseptic) — Антисептики и дезинфицирующие средства<br> 20 (petfood) — Корма для животных<br> 21 (seafood) — Морепродукты <br>22 (nabeer) — Безалкогольное пиво <br>23 (softdrinks) — Соковая продукция и безалкогольные напитки<br> 26 (vetpharma) — Ветеринарные препараты <br>27 (toys) — Игры и игрушки для детей<br> 28 (radio) — Радиоэлектронная продукция<br> 31 (titan) — Титановая металлопродукция<br> 32 (conserve) — Консервированная продукция<br> 33 (vegetableoil) — Растительные масла<br> 34 (opticfiber) — Оптоволокно и оптоволоконная продукция<br> 35 (chemistry) — Парфюмерные и косметические средства и бытовая химия<br> 38 (pharmaraw) — Фармацевтическое сырьё, лекарственные средства | [8] |
| verified           | bool      | - | Результат криптографической проверки кода. Для всех товарных групп, кроме "Товары из натурального меха": <br>true — проверка крипто-подписи успешна,<br>false — проверка крипто-подписи завершилась с ошибкой. <br><br>Для "Товаров из натурального меха": true — КиЗ найден в "ИР Маркировки". <br>false —  КиЗ не найден в "ИР Маркировки". | true              |
| realizable         | bool      | - | Признак того, что код находится в статусе "В обороте" | true              |
| utilised           | bool      | - | Признак нанесения кода на упаковку товара | true              |
| found              | bool      | - | Признак того, что код найден в ГИС МТ | true              |
| errorCode | int | - | Код ошибки. <br>0 — ошибки отсутствуют; <br>1 — «Ошибка валидации КМ»; <br>2 — "КМ не содержит GTIN"; <br>3 — "КМ не содержит серийный номер"; <br>4 — "КМ содержит недопустимые символы"; <br>5 — "Ошибка верификации крипто-подписи КМ(формат крипто-подписи не соответствует типу КМ)"; <br>6 — "Ошибка верификации крипто-подписи КМ(крипто-подпись не валидная)"; <br>7 — "Ошибка верификации крипто-подписи КМ(крипто-ключ не валиден)"; <br>8 — "КМ не прошел верификацию в стране эмитента"; <br>9 — "Найденные AI в КМ не поддерживаются"; <br>10 — "КМ не найден в ГИС МТ" | 0 |
| message                | string    | - | Сообщение об ошибке | null |
| isTracking | bool | - | Признак старта прослеживаемости в товарной группе. <br>true — прослеживаемость в товарной группе для данного КИ / КиЗ включена; <br>false — прослеживаемость в товарной группе для данного КИ /КиЗ не включена | true |
| sold | bool      | - | Признак того, что товар с данным кодом уже продан | true |
| gtin               | string    | - | Код товара (GTIN) | "04670540176099" |
| packageType | string | - | Тип упаковки. <br>Для товарной группы "Товары из натурального меха" значение всегда UNIT. <br><br>UNIT, Единица товара (КИ) — Пачка, Потребительская упаковка; <br>GROUP, Групповая — упаковка (КИГУ); <br>SET — Набор (КИН); <br>BUNDLE, Комплект (КИК) — Комплект (КИК). Используется только для товарной группы "Предметы одежды, бельё постельное, столовое, туалетное и кухонное"; <br>BOX — Транспортная упаковка (КИТУ); <br>ATK — Агрегированный таможенный код (АТК). <br>В составе АТК может быть: <br>- КИ; <br>- КИК; <br>- КИГУ (кроме товарной группы "Медицинские изделия"); <br>- КИТУ (только для товарных групп "Антисептики и дезинфицирующие средства", "Биологически активные добавки к пище", "Медицинские изделия", "Пиво, напитки, изготавливаемые на основе пива, слабоалкогольные напитки", "Соковая продукция и безалкогольные напитки", "Упакованная вода"). <br><br>Возвращаются в ответе: <br> LEVEL1 — Транспортная упаковка 1-го уровня (КИТУ). Также может быть групповой упаковкой (КИГУ) —  блок. <br>LEVEL2 — Транспортная упаковка 2-го уровня(КИТУ), Короб. <br>LEVEL3 — Транспортная упаковка 3-го уровня (КИТУ), Палета. <br>LEVEL4 — Транспортная упаковка 4-го уровня(КИТУ). <br>LEVEL5 — Транспортная упаковка 5-го уровня(КИТУ)| "UNIT" |
| producerInn | string | - | ИНН производителя | "7725344604" |
| grayZone | bool | - | Признак нахождения продукции в "серой зоне" (только для товарных групп "Альтернативная табачная продукция", "Никотиносодержащая продукция", "Табачная продукция"). <br>Возможные значения: <br> true —  находится в серой зоне; <br> false —  не находится в серой зоне. | false |
| isBlocked          | bool      | - | Признак блокировки кода по решению органа государственной власти (ОГВ) | true  |
| isGreyGtin         | bool      | - | Признак некорректного (не зарегистрированного в справочниках) GTIN в составе кода | true |
| ogvs | string[] | - | Органы государственной власти, установившие блокировку кода (заполняется при **isBlocked** = true): <br>RAR — Росалкогольрегулирование; <br>FTS — ФТС России; <br>FNS — ФНС России; <br>RSHN — Россельхознадзор; <br>RPN — Роспотребнадзор; <br>MVD — МВД России; <br>RZN — Росздравнадзор.  | [] |
| found          | bool      | - | Признак наличия КМ в ГИС МТ. <br>true — КМ найден; <br>false —  КМ не найден | true  |
| packageQuantity | int | - | Ёмкость КИГУ. <br>Количество потенциально вмещаемых вложений | 0 |

#### CheckTemplateDocument

| **Имя поля**    | **Тип**                     | **Обяз** | **Назначение**                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    | **Пример**                                                                                             |
| --------------- | --------------------------- | -------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------ |
| TaskType        | int                         | -        | Тип чека.  <br>0 — Текст;  <br>1 — Продажа;  <br>2 — Возврат;  <br>3 — Покупка;  <br>4 — ВозвратПокупки;  <br>5 — ЧекКоррекцииПрихода;  <br>6 — ЧекКоррекцииВозвратаПрихода;  <br>7 — ЧекКоррекцииРасхода;  <br>8 — ЧекКоррекцииВозвратаРасхода                                                                                                                                                                                                                                                                   | 1                                                                                                      |
| TaxType         | int                         | -        | Система налогообложения: <br>0 — Общая (ОСН); <br>1 — Упрощенная Доход(УСН); <br>2 — Упрощенная Доход минус Расход(УСНД_Р); <br>3 — Единый налог на вмененный доход(ЕНВД); <br>4 — Единый сельскохозяйственный налог(ЕСН); <br>5 — Патентная система налогообложения(ПСН)                                                                                                                                                                                                                                         | 0                                                                                                      |
| TimeZone        | int                         | -        | Номер часовой зоны места расчета: <br>0 — не выбрано; <br>1 — 1 часовая зона (МСК-1 / UTC+2); <br>2 — 2 часовая зона (МСК / UTC+3); <br>3 — 3 часовая зона (МСК+1 / UTC+4); <br>4 — 4 часовая зона (МСК+2 / UTC+5); <br>5 — 5 часовая зона (МСК+3 / UTC+6); <br>6 — 6 часовая зона (МСК+4 / UTC+7); <br>7 — 7 часовая зона (МСК+5 / UTC+8); <br>8 — 8 часовая зона (МСК+6 / UTC+9); <br>9 — 9 часовая зона (МСК+7 / UTC+10); <br>10 — 10 часовая зона (МСК+8 / UTC+11); <br>11 — 11 часовая зона (МСК+9 / UTC+12) | 9                                                                                                      |
| OperationOnline | bool                        | -        | Признак применения ККТ при расчёте в безналичном порядке в сети «Интернет»                                                                                                                                                                                                                                                                                                                                                                                                                                        | false                                                                                                  |
| IsReplaceTax    | bool                        | -        | Признак замены системы налогообложения настройками ККТ                                                                                                                                                                                                                                                                                                                                                                                                                                                            | true                                                                                                   |
| TrustedInFn     | bool                        | -        | Подтвержден в ФН                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  | false                                                                                                  |
| Sum             | decimal                     | -        | Сумма с учетом скидки                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | 0.0                                                                                                    |
| Change          | decimal                     | -        | Сдача                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | 0                                                                                                      |
| Electronically  | bool                        | -        | Регистрация чека без печати на ленте                                                                                                                                                                                                                                                                                                                                                                                                                                                                              | false                                                                                                  |
| IsFiscal        | bool                        | -        | Фискальный режим                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  | true                                                                                                   |
| MtNumber        | int                         | -        | Номер документа "Уведомление о реализации МТ" в который включается данные чека                                                                                                                                                                                                                                                                                                                                                                                                                                    | 0                                                                                                      |
| PrintError      | bool                        | -        | Ошибка при печати бумажной формы чека                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | false                                                                                                  |
| CheckItems      | [CheckItem](#checkitem)[] | -        | Позиции шаблона                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   | \[\]                                                                                                   |
| Payments        | [Payments](#payments)     | -        | Способы оплаты                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    | "Payments": {<br>"Cash": 101,<br>"Electronic": 0,<br>"PrePaid": 0,<br>"Credit": 0,<br>"Barter": 0<br>} |

#### DeviceTask

| **Имя поля**      | **Тип**                   | **Обяз** | **Назначение**                                                                                                                                                                                                                                                                                                                                                                                               | **Пример**                             |
| ----------------- | ------------------------- | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------- |
| TaskType          | int                       | +        | Тип задания <br>Тип задания:  <br>1 — Приход;  <br>2 — Возврат прихода;  <br>3 — Расход;  <br>4 — Возврат расхода;  <br>5 — Коррекция прихода;  <br>6 — Коррекция возврата прихода;  <br>7 — Коррекция расхода;  <br>8 — Коррекция возврата расхода;  <br>9 — Слип;  <br>11 — Открытие смены;  <br>12 — Z-отчёт;  <br>13 — X-отчёт;  <br>14 — Отчёт о состоянии расчётов;  <br>22 — Открытие денежного ящика | 1                                      |
| DocId             | string                    | +        | Идентификатор документа                                                                                                                                                                                                                                                                                                                                                                                      | "567e2bde-9fb2-4fde-bac4-ceb6aed54bb4" |
| Date              | datetime                  | +        | Дата создания документа по часам ПК; после обработки — дата и время выполнения операции                                                                                                                                                                                                                                                                                                                      | "2026-08-31T14:49:55+08:00"            |
| BaseDocId         | string                    | –        | Id документа-основания (для связывания операций в цепочку)                                                                                                                                                                                                                                                                                                                                                   | "914a7c5c-4233-4dab-b839-eccefefe3421" |
| TerminalId        | string                    | +        | Идентификатор терминала, с которого пришёл документ                                                                                                                                                                                                                                                                                                                                                          | "WEB_Admin_127.0.0.1"                  |
| RequestId         | string                    | –        | Идентификатор запроса на запись документа (заголовок `x-request-id`); защита от повторной записи                                                                                                                                                                                                                                                                                                             | "…"                                    |
| DeviceName        | string                    | +        | Имя устройства                                                                                                                                                                                                                                                                                                                                                                                               | "Emu"                                  |
| PoolId            | string                    | –        | Пул, назначенный документу (печать на любой ККМ пула)                                                                                                                                                                                                                                                                                                                                                        | ""                                     |
| ResultCode        | int                       | +        | Код результата (`0` — успех)                                                                                                                                                                                                                                                                                                                                                                                 | 0                                      |
| ResultDescription | string                    | +        | Описание результата                                                                                                                                                                                                                                                                                                                                                                                          | "OK"                                   |
| Processed         | bool                      | +        | Признак удачного завершения обработки                                                                                                                                                                                                                                                                                                                                                                        | true                                   |
| ClientVersion     | string                    | –        | Версия клиента (драйверы 1С прописывают свою)                                                                                                                                                                                                                                                                                                                                                                | null                                   |
| ServerVersion     | string                    | +        | Версия сервера (проставляется автоматически)                                                                                                                                                                                                                                                                                                                                                                 | "4.0.70.827"                           |
| DeviceInfo        | [DeviceInfo](#deviceinfo) | –        | Информация о модели устройства, обработавшего задание                                                                                                                                                                                                                                                                                                                                                        | {"Model": "РБ-Софт:Эмулятор ККМ", …}   |
| SenderInfo        | SenderInfo                | –        | Информация о приложении-источнике запроса                                                                                                                                                                                                                                                                                                                                                                    | {"AppName": "1С", "AppVersion": "4.7"} |


