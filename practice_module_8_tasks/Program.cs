/* Курс: Шаблоны проектирования приложений

Тема: Модуль 08 Структурные паттерны. Декоратор. Адаптер
Баллы: 
Задача:
Реализуйте систему управления отчетностью для интернет-магазина с применением паттерна "Декоратор". Система должна поддерживать генерацию различных отчетов по продажам и пользователям с возможностью добавления дополнительных функциональных возможностей, таких как фильтрация по датам, сортировка по определенным критериям, экспорт данных в различные форматы (например, в CSV и PDF). В основе должна быть гибкая архитектура, которая позволит динамически добавлять новые "декорации" без изменения исходного кода базовых классов.
Структура работы:
1. Базовые требования:
•	Интерфейс IReport: Интерфейс для генерации отчетов, содержащий метод Generate(), возвращающий строку с данными отчета.
•	Классы отчетов:
o	SalesReport — класс, представляющий отчет по продажам.
o	UserReport — класс, представляющий отчет по пользователям.
2. Декораторы:
•	Фильтр по датам: Декоратор DateFilterDecorator добавляет фильтрацию данных по указанному диапазону дат.
•	Сортировка данных: Декоратор SortingDecorator добавляет возможность сортировки данных по определенному критерию (например, по дате или сумме продажи).
•	Экспорт:
o	Декоратор CsvExportDecorator добавляет возможность экспорта отчета в формат CSV.
o	Декоратор PdfExportDecorator добавляет возможность экспорта отчета в PDF.
3. Дополнительные требования:
•	Добавьте возможность применения нескольких декораторов к одному отчету.
•	Реализуйте клиентский код, который создает отчет и применяет к нему декораторы в зависимости от требований пользователя.
•	Генерируйте данные отчетов с фиктивной информацией для демонстрации работы.
Детали реализации:
1.	Интерфейс для отчетов: Создайте интерфейс IReport с методом Generate(), который возвращает строку.
2.	Классы отчетов: Реализуйте классы отчетов, такие как SalesReport и UserReport, которые наследуют IReport и возвращают соответствующие данные.
3.	Абстрактный декоратор: Создайте абстрактный класс ReportDecorator, который наследует IReport и содержит ссылку на базовый отчет.
4.	Декораторы: Фильтрация по датам, Сортировка данных, Экспорт в CSV, Экспорт в PDF
5.	Клиентский код

Задания:
1.	Реализуйте предложенную архитектуру.
2.	Добавьте новые декораторы, такие как фильтрация по сумме продаж или пользователям с определенными характеристиками.
3.	Создайте механизм динамического выбора декораторов в зависимости от пользовательских запросов.
4.	Протестируйте систему с различными комбинациями отчетов и декораторов.*/
/*using System;

namespace ReportDecoratorApp
{
    public interface IReport
    {
        string Generate();
    }

    public class SalesReport : IReport
    {
        public string Generate()
        {
            return "Sales Report: Дархан - 500 тенге, Пётр - 1200 тенге, Акжан - 800 тенге";
        }
    }

    public class UserReport : IReport
    {
        public string Generate()
        {
            return "User Report: Дархан (KZ), Пётр (RU), Акжан (KZ)";
        }
    }

    public abstract class ReportDecorator : IReport
    {
        protected IReport _report;
        public ReportDecorator(IReport report) { _report = report; }
        public virtual string Generate() => _report.Generate();
    }

    public class DateFilterDecorator : ReportDecorator
    {
        private string _from, _to;
        public DateFilterDecorator(IReport report, string from, string to) : base(report)
        {
            _from = from; _to = to;
        }
        public override string Generate()
        {
            return _report.Generate() + $" | Filtered by date: {_from} - {_to}";
        }
    }

    public class SortingDecorator : ReportDecorator
    {
        private string _criteria;
        public SortingDecorator(IReport report, string criteria) : base(report)
        {
            _criteria = criteria;
        }
        public override string Generate()
        {
            return _report.Generate() + $" | Sorted by: {_criteria}";
        }
    }

    public class CsvExportDecorator : ReportDecorator
    {
        public CsvExportDecorator(IReport report) : base(report) { }
        public override string Generate()
        {
            return _report.Generate() + " | Exported to CSV";
        }
    }

    public class PdfExportDecorator : ReportDecorator
    {
        public PdfExportDecorator(IReport report) : base(report) { }
        public override string Generate()
        {
            return _report.Generate() + " | Exported to PDF";
        }
    }

    public class SalesAmountFilterDecorator : ReportDecorator
    {
        private double _minAmount;
        public SalesAmountFilterDecorator(IReport report, double minAmount) : base(report)
        {
            _minAmount = minAmount;
        }
        public override string Generate()
        {
            return _report.Generate() + $" | Filtered by sales >= {_minAmount} тенге";
        }
    }

    public class UserRegionFilterDecorator : ReportDecorator
    {
        private string _region;
        public UserRegionFilterDecorator(IReport report, string region) : base(report)
        {
            _region = region;
        }
        public override string Generate()
        {
            return _report.Generate() + $" | Filtered by region: {_region}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Дархан запросил отчёт по продажам:");
            IReport report1 = new SalesReport();
            report1 = new DateFilterDecorator(report1, "01.10.2025", "31.10.2025");
            report1 = new SortingDecorator(report1, "Сумма");
            report1 = new CsvExportDecorator(report1);
            Console.WriteLine(report1.Generate());

            Console.WriteLine("\nПётр запросил отчёт по пользователям:");
            IReport report2 = new UserReport();
            report2 = new UserRegionFilterDecorator(report2, "KZ");
            report2 = new PdfExportDecorator(report2);
            Console.WriteLine(report2.Generate());

            Console.WriteLine("\nАкжан запросил детализированный отчёт:");
            IReport report3 = new SalesReport();
            report3 = new SalesAmountFilterDecorator(report3, 700);
            report3 = new DateFilterDecorator(report3, "01.11.2025", "05.11.2025");
            report3 = new SortingDecorator(report3, "Дата");
            report3 = new PdfExportDecorator(report3);
            Console.WriteLine(report3.Generate());
        }
    }
}*/




/* Задача:
Вам необходимо разработать систему для мониторинга логистики и обработки заказов в крупной сети складов. На текущий момент система поддерживает взаимодействие с внутренней службой доставки. В то же время необходимо интегрировать сторонние службы логистики, каждая из которых имеет свой собственный интерфейс. Для решения этой задачи необходимо использовать паттерн "Адаптер".
Описание:
1.	В системе есть существующий интерфейс для внутренней службы доставки:
o	IInternalDeliveryService, который реализован в классе InternalDeliveryService.
2.	Вам нужно интегрировать несколько сторонних логистических служб:
o	Сторонние службы логистики имеют собственные интерфейсы и методы, такие как ExternalLogisticsServiceA и ExternalLogisticsServiceB.
3.	Вы должны разработать адаптеры для каждой из сторонних служб:
o	Адаптеры должны преобразовывать интерфейсы внешних служб логистики к единому интерфейсу внутренней системы.
4.	Кроме того, необходимо создать фабрику, которая будет динамически возвращать нужный тип службы доставки в зависимости от требований пользователя (внутренняя или внешняя служба).
Структура работы:
1. Основные компоненты:
•	Интерфейс IInternalDeliveryService: Интерфейс для внутренней службы доставки с методами DeliverOrder(string orderId) и GetDeliveryStatus(string orderId).
•	Класс InternalDeliveryService: Класс, который реализует интерфейс IInternalDeliveryService и симулирует процесс доставки заказа.
2. Сторонние службы логистики:
•	ExternalLogisticsServiceA: Сторонняя служба, которая имеет метод ShipItem(int itemId) и TrackShipment(int shipmentId) для отслеживания отправки.
•	ExternalLogisticsServiceB: Сторонняя служба с методами SendPackage(string packageInfo) и CheckPackageStatus(string trackingCode).
3. Адаптеры:
•	LogisticsAdapterA: Адаптер, который позволяет интегрировать ExternalLogisticsServiceA в систему через интерфейс IInternalDeliveryService.
•	LogisticsAdapterB: Адаптер для интеграции ExternalLogisticsServiceB.
4. Фабрика:
•	DeliveryServiceFactory: Фабрика, которая возвращает нужный тип службы доставки (внутреннюю или стороннюю) в зависимости от пользовательских требований.

Детали реализации:
1.	Интерфейс и класс для внутренней службы доставки
2.	Сторонние логистические службы
3.	Адаптеры для внешних служб логистики: Адаптер для ExternalLogisticsServiceA, Адаптер для ExternalLogisticsServiceB
4.	Фабрика для выбора службы доставки
5.	Клиентский код

Задания:
1.	Реализуйте предложенную архитектуру, используя паттерн "Адаптер".
2.	Добавьте еще одну стороннюю логистическую службу с уникальным интерфейсом и создайте для нее адаптер.
3.	Расширьте функционал фабрики для обработки дополнительных типов служб доставки.
4.	Реализуйте обработку ошибок и логгирование процесса доставки в адаптерах.
5.	Добавьте функционал для расчета стоимости доставки, который также должен поддерживаться через адаптеры. */
/*using System;

namespace LogisticsAdapterApp
{
    public interface IInternalDeliveryService
    {
        void DeliverOrder(string orderId);
        string GetDeliveryStatus(string orderId);
        double CalculateDeliveryCost(double weight);
    }

    public class InternalDeliveryService : IInternalDeliveryService
    {
        public void DeliverOrder(string orderId)
        {
            Console.WriteLine($"Внутренняя служба: заказ {orderId} доставлен.");
        }

        public string GetDeliveryStatus(string orderId)
        {
            return $"Внутренняя служба: статус заказа {orderId} — доставлено.";
        }

        public double CalculateDeliveryCost(double weight)
        {
            return weight * 100;
        }
    }

    public class ExternalLogisticsServiceA
    {
        public void ShipItem(int itemId)
        {
            Console.WriteLine($"External A: отправлен товар {itemId}.");
        }

        public string TrackShipment(int shipmentId)
        {
            return $"External A: статус доставки {shipmentId} — в пути.";
        }

        public double GetShippingCost(int weight)
        {
            return weight * 120;
        }
    }

    public class ExternalLogisticsServiceB
    {
        public void SendPackage(string packageInfo)
        {
            Console.WriteLine($"External B: посылка '{packageInfo}' отправлена.");
        }

        public string CheckPackageStatus(string trackingCode)
        {
            return $"External B: статус '{trackingCode}' — доставлено.";
        }

        public double ComputeCost(double weight)
        {
            return weight * 90;
        }
    }

    public class ExternalLogisticsServiceC
    {
        public void StartDelivery(string code, string address)
        {
            Console.WriteLine($"External C: доставка {code} начата для {address}.");
        }

        public string DeliveryState(string code)
        {
            return $"External C: доставка {code} в процессе.";
        }

        public double DeliveryPrice(double distance)
        {
            return distance * 50;
        }
    }

    public class LogisticsAdapterA : IInternalDeliveryService
    {
        private ExternalLogisticsServiceA _serviceA;
        public LogisticsAdapterA(ExternalLogisticsServiceA serviceA)
        {
            _serviceA = serviceA;
        }

        public void DeliverOrder(string orderId)
        {
            try
            {
                int id = int.Parse(orderId);
                _serviceA.ShipItem(id);
            }
            catch
            {
                Console.WriteLine("Ошибка при отправке заказа через External A.");
            }
        }

        public string GetDeliveryStatus(string orderId)
        {
            try
            {
                int id = int.Parse(orderId);
                return _serviceA.TrackShipment(id);
            }
            catch
            {
                return "Ошибка получения статуса через External A.";
            }
        }

        public double CalculateDeliveryCost(double weight)
        {
            return _serviceA.GetShippingCost((int)weight);
        }
    }

    public class LogisticsAdapterB : IInternalDeliveryService
    {
        private ExternalLogisticsServiceB _serviceB;
        public LogisticsAdapterB(ExternalLogisticsServiceB serviceB)
        {
            _serviceB = serviceB;
        }

        public void DeliverOrder(string orderId)
        {
            _serviceB.SendPackage(orderId);
        }

        public string GetDeliveryStatus(string orderId)
        {
            return _serviceB.CheckPackageStatus(orderId);
        }

        public double CalculateDeliveryCost(double weight)
        {
            return _serviceB.ComputeCost(weight);
        }
    }

    public class LogisticsAdapterC : IInternalDeliveryService
    {
        private ExternalLogisticsServiceC _serviceC;
        public LogisticsAdapterC(ExternalLogisticsServiceC serviceC)
        {
            _serviceC = serviceC;
        }

        public void DeliverOrder(string orderId)
        {
            _serviceC.StartDelivery(orderId, "Алматы");
        }

        public string GetDeliveryStatus(string orderId)
        {
            return _serviceC.DeliveryState(orderId);
        }

        public double CalculateDeliveryCost(double distance)
        {
            return _serviceC.DeliveryPrice(distance);
        }
    }

    public class DeliveryServiceFactory
    {
        public static IInternalDeliveryService GetService(string type)
        {
            if (type == "internal") return new InternalDeliveryService();
            if (type == "A") return new LogisticsAdapterA(new ExternalLogisticsServiceA());
            if (type == "B") return new LogisticsAdapterB(new ExternalLogisticsServiceB());
            if (type == "C") return new LogisticsAdapterC(new ExternalLogisticsServiceC());
            throw new Exception("Неизвестный тип службы доставки.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Дархан оформил доставку через внутреннюю службу:");
            var service1 = DeliveryServiceFactory.GetService("internal");
            service1.DeliverOrder("101");
            Console.WriteLine(service1.GetDeliveryStatus("101"));
            Console.WriteLine($"Стоимость: {service1.CalculateDeliveryCost(2.5)} тенге");

            Console.WriteLine("\nПётр оформил доставку через внешнюю службу A:");
            var service2 = DeliveryServiceFactory.GetService("A");
            service2.DeliverOrder("202");
            Console.WriteLine(service2.GetDeliveryStatus("202"));
            Console.WriteLine($"Стоимость: {service2.CalculateDeliveryCost(3)} тенге");

            Console.WriteLine("\nАкжан оформил доставку через внешнюю службу B:");
            var service3 = DeliveryServiceFactory.GetService("B");
            service3.DeliverOrder("Посылка-303");
            Console.WriteLine(service3.GetDeliveryStatus("Посылка-303"));
            Console.WriteLine($"Стоимость: {service3.CalculateDeliveryCost(4)} тенге");

            Console.WriteLine("\nПётр оформил доставку через службу C:");
            var service4 = DeliveryServiceFactory.GetService("C");
            service4.DeliverOrder("C404");
            Console.WriteLine(service4.GetDeliveryStatus("C404"));
            Console.WriteLine($"Стоимость: {service4.CalculateDeliveryCost(10)} тенге");
        }
    }
}*/