# WPF Warehouse — Система управления складом

Десктопное приложение на **WPF (C#)** для учёта товаров на складе. 
Работает с базой данных **SQL Server** через **Entity Framework (Database First)**.

## Возможности

- Полный CRUD для 4 таблиц: **Категории**, **Поставщики**, **Товары**, **Поставки**
- Поиск по каждой таблице
- Валидация всех полей ввода с понятными сообщениями об ошибках
- Выпадающие списки для внешних ключей (категория, поставщик, товар)
- Каскадное удаление связанных записей
- Автоматическое обновление DataGrid после добавления/изменения/удаления
- Автоматический пересчёт количества товара при работе с поставками
- Удобная навигация между разделами

## Структура базы данных

| Таблица | Описание | Связи |
|---------|----------|-------|
| `categories` | Категории товаров | 1 → N товаров |
| `suppliers` | Поставщики | 1 → N товаров |
| `products` | Товары | N → 1 категория, N → 1 поставщик |
| `deliveries` | Поставки | N → 1 товар |

Схема нормализована до **3-й нормальной формы (3НФ)**.

## Стек технологий

- **C# / .NET Framework**
- **WPF** (Windows Presentation Foundation)
- **Entity Framework 6** (Database First, EDMX)
- **SQL Server** (LocalDB / Express)

## Запуск проекта

### 1. Создайте базу данных

Выполните SQL-скрипт из файла [`database.sql`](database.sql) в SQL Server Management Studio:

```sql
CREATE DATABASE WpfWarehouseDb;
GO
USE WpfWarehouseDb;
GO
-- ... (полный скрипт создания таблиц и тестовых данных)
```

### 2. Настройте строку подключения

Откройте `App.config` и проверьте строку подключения:

```xml
<connectionStrings>
    <add name="WpfWarehouseDbEntities"
         connectionString="metadata=res://*/Model1.csdl|res://*/Model1.ssdl|res://*/Model1.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost\SQLEXPRESS;initial catalog=WpfWarehouseDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;"
         providerName="System.Data.EntityClient" />
</connectionStrings>
```

Измените `data source` под ваш экземпляр SQL Server:
- `localhost\SQLEXPRESS` — SQL Server Express
- `.` или `(localdb)\MSSQLLocalDB` — LocalDB
- `ИМЯ_СЕРВЕРА` — удалённый сервер

### 3. Соберите и запустите

Откройте `WpfWarehouse.sln` в **Visual Studio 2019/2022**, восстановите NuGet-пакеты и нажмите **F5**.

## Структура проекта

```
WpfWarehouse/
├── Services/
│   └── DatabaseService.cs      # Слой доступа к данным (CRUD + поиск)
├── Views/                      # Страницы (UserControl)
│   ├── CategoriesView.xaml
│   ├── SuppliersView.xaml
│   ├── ProductsView.xaml
│   └── DeliveriesView.xaml
├── Windows/                    # Диалоговые окна добавления/редактирования
│   ├── CategoryDialog.xaml
│   ├── SupplierDialog.xaml
│   ├── ProductDialog.xaml
│   └── DeliveryDialog.xaml
├── Model1.edmx                 # EF-модель (Database First)
├── categories.cs               # Сгенерированные сущности
├── suppliers.cs
├── products.cs
├── deliveries.cs
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs
└── App.config
```

## Основные сценарии работы

1. **Добавление поставки** автоматически увеличивает количество товара на складе.
2. **Редактирование поставки** корректно пересчитывает остаток (учитывает и старое, и новое значение).
3. **Удаление поставки** уменьшает количество товара.
4. **Удаление категории/поставщика** каскадно удаляет связанные товары и их поставки.

## Требования

- Windows 10/11
- Visual Studio 2019 или новее (с workload «.NET desktop development»)
- SQL Server Express / LocalDB
- .NET Framework 4.7.2+

## Лицензия

Учебный проект. Свободно используйте в образовательных целях.
