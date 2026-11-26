# SKKM Connector CSharp

Библиотека `SkkmConnector` — обёртка над REST API для приложений на .NET 6.

## Возможности

- **Полное покрытие REST API Сервера ККМ 4.0**.
- **Объектный доступ к функциям Сервера ККМ** — программисту нет необходимости выполнять запросы к REST API, требуется создать объект `SkkmConnector` и вызывать его методы.

Полный список методов и какие свойства они изменяют — в [Методы](Методы.md). Примеры их использования и вывода — в [Примеры использования.md](Примеры%20использования.md).

## Требования

- .NET 6
- Сервер ККМ 4.0 (порт по умолчанию `4398`)
- `Token` — только если на сервере выключен анонимный доступ

## Использование

Один экземпляр `SkkmConnector` — одна рабочая сессия.

1. Задайте подключение: адрес, токен, имя кассы, кассира.
2. Перед новым чеком вызовите `NewCheck` — очистятся позиции, оплаты, покупатель и коррекция. Кассир, `PaymentType` и `TaxVariant` не сбрасываются.
3. Заполните свойства под операцию.
4. Вызовите метод.
5. При ошибке читайте `ErrorCode` и `ErrorDescription`.

```csharp
if (!await connector.PrintCheck())
    throw new InvalidOperationException($"{connector.ErrorCode}: {connector.ErrorDescription}");
```

Ограничения:

- коннектор не проверяет состав чека;
- после `Dispose()` экземпляр использовать нельзя.

### Параметры подключения

| Свойство     | Тип    | Обязательный | По умолчанию     | Назначение                                                                                                     |
| ------------ | ------ | ------------ | ---------------- | -------------------------------------------------------------------------------------------------------------- |
| Address      | string | +            | localhost:4398   | Адрес сервера ККМ (`хост` или `хост:порт`). Порт можно опустить — тогда 4398.                                  |
| Token        | string | -            |                  | Токен авторизации (`api_key`). <br>Если включен анонимный доступ к REST API, то указывать токен необязательно. |
| DeviceName   | string | +            |                  | Имя устройства на сервере                                                                                      |
| CashierName  | string | -            |                  | ФИО кассира                                                                                                    |
| CashierVatin | string | -            |                  | ИНН кассира                                                                                                    |
| TerminalId   | string | -            |                  | Идентификатор рабочего места (`terminalId`)                                                                    |

### Примеры

#### Получение подробной информации о ККТ

```csharp
using RBSoftSkkm;

using var connector = new SkkmConnector();
connector.Address = "localhost";
connector.DeviceName = "Emulator";
connector.Token = "a6261fa7-6675-41c6-a9f5-cee920e1d71c";

if (await connector.GetKktInfo())
    Console.WriteLine($"ФФД: {connector.DeviceFfdVersion}, строка: {connector.DeviceLineLength}");
else
    Console.WriteLine($"{connector.ErrorCode}: {connector.ErrorDescription}");
```

#### Открытие смены

```csharp
using RBSoftSkkm;

using var connector = new SkkmConnector();
connector.Address = "localhost";
connector.DeviceName = "Emulator";
connector.CashierName = "Иванов И.В.";
connector.CashierVatin = "520205004556";

if (await connector.OpenShift())
    Console.WriteLine($"Смена открыта: {connector.ShiftNumber}");
else
    Console.WriteLine($"{connector.ErrorCode}: {connector.ErrorDescription}");
```

#### Чек прихода

```csharp
using RBSoftSkkm;

using var connector = new SkkmConnector();
connector.Address = "localhost";
connector.DeviceName = "Emulator";
connector.CashierName = "Иванов И.В.";

connector.NewCheck(CheckType.Sale, TaxSystem.Osn);

connector.AddPosition(
    "Кофе",
    2,
    50,
    100,
    TaxRate.Vat22,
    SignCalculationObject.Goods,
    SignMethodCalculation.FullPayment,
    MeasureOfQuantity.Piece);

connector.Cash = 100;

if (await connector.PrintCheck())
    Console.WriteLine($"ФП: {connector.FiscalSign}, смена: {connector.ShiftNumber}, DocId: {connector.DocId}");
else
    Console.WriteLine($"{connector.ErrorCode}: {connector.ErrorDescription}");
```

## Установка

Пространство имён: `RBSoftSkkm`.

В демо-приложении всегда лежит последняя версия коннектора. Сборка и подключение пакета ниже — для тех, кто внёс свою доработку в коннектор и подключает её к своему проекту.

Пакет в nuget.org не публикуется. Подключайте локальный `.nupkg` из папки `nupkg/` или ссылку на проект.

### Собрать пакет

Из корня репозитория:

```powershell
dotnet pack SkkmConnector\SkkmConnector.csproj -c Release -o nupkg
```

В `nupkg/` появится файл вида `SkkmConnector.1.34.26.nupkg`. Версию задаёт `Version` в `SkkmConnector.csproj`.

### Подключить NuGet-пакет

Из каталога вашего приложения, рядом с папкой `nupkg`:

```powershell
dotnet add package SkkmConnector --version 1.34.26 --source ".\nupkg"
```

Либо один раз прописать источник в `nuget.config` рядом с `.csproj` или `.sln`:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="local-skkm" value="nupkg" />
  </packageSources>
</configuration>
```

После этого в проекте достаточно:

```xml
<PackageReference Include="SkkmConnector" Version="1.34.26" />
```

```powershell
dotnet restore
```

Если `nupkg` лежит не рядом с проектом, в `nuget.config` укажите полный путь к папке.

### Ссылка на проект

Когда коннектор лежит в том же решении и пакет собирать не нужно:

```xml
<ProjectReference Include="..\SkkmConnector\SkkmConnector.csproj" />
```
