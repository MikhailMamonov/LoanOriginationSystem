# 🏦 Loan Origination System (LOS)

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-15-336791?logo=postgresql)](https://www.postgresql.org/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

**Enterprise-grade Loan Origination System**, разработанный с применением принципов Domain-Driven Design (DDD), Clean Architecture и CQRS. Проект демонстрирует лучшие практики разработки высоконагруженных и отказоустойчивых банковских систем.

---

## 📌 О проекте

Цель данного проекта — создание масштабируемого ядра системы кредитования, способного управлять жизненным циклом заявки (от черновика до выдачи), проверять сложные бизнес-правила (скоринг, ПДН, стаж) и гарантировать целостность данных. 

Архитектура спроектирована с учетом требований регуляторов и стандартов финансовой индустрии: строгая валидация инвариантов, аудиторский след (Timeline), изоляция доменной логики и готовность к интеграции с внешними системами (БКИ, Camunda BPM).

---

## 🏗 Архитектура и ключевые паттерны

Проект строго следует принципам **Clean Architecture** и **SOLID**:

*   **Domain-Driven Design (DDD):** 
    *   Четкое разделение на Aggregate Roots (`Client`, `LoanProduct`, `LoanApplication`), Entity и Value Objects (`EligibilityCriteria`, `ProductFees`).
    *   Защита бизнес-инвариантов внутри методов агрегатов (например, запрет на изменение заявки после выдачи).
    *   Паттерн **Specification** для инкапсуляции правил доступности продукта.
*   **CQRS & Mediator:** Разделение операций чтения (Queries) и записи (Commands) с использованием библиотеки `MediatR`. Это обеспечивает тонкие контроллеры и легкую расширяемость.
*   **Dependency Inversion:** Слой `Application` зависит только от абстракций (интерфейсов репозиториев), что позволяет легко подменять реализацию (например, для Unit-тестов).
*   **Unit of Work:** Координация транзакций для гарантии атомарности операций сохранения.
*   **State Machine:** Управление статусами заявки (`Draft` → `Submitted` → `Scoring` → `Approved`/`Rejected`) с защитой от недопустимых переходов через кастомные исключения (`InvalidEntityStateException`).

---

## 🛠 Технологический стек

| Категория | Технологии |
| :--- | :--- |
| **Backend** | C# 12, .NET 8, ASP.NET Core Web API |
| **Архитектура** | Clean Architecture, DDD, CQRS, MediatR |
| **База данных** | PostgreSQL 15, Entity Framework Core 8 (Code First, Owned Types) |
| **Контейнеризация** | Docker, Docker Compose |
| **Тестирование** | xUnit, Moq, FluentAssertions, Bogus |
| **Документация** | Swagger / OpenAPI 3.0 |

---

## 🚀 Быстрый старт

Для локального запуска проекта требуется установленный **Docker** и **.NET 8 SDK**.

### 1. Клонирование репозитория
```bash
git clone https://github.com/MikhailMamonov/LoanOriginationSystem.git
cd LoanOriginationSystem
```
---
### 2. Запуск инфраструктуры (PostgreSQL)

```bash
docker-compose up -d
```
---
### 3. Применение миграций базы данных

```bash
cd LOS.API
dotnet ef database update --project ../LOS.Infrastructure
```
---
### 4. Запуск 
```bash
dotnet run
```
---
### 5. Доступ к API
Откройте браузер и перейдите по адресу:<br>
👉 https://localhost:5000/swagger (порт может отличаться в зависимости от launchSettings.json)
---
### 6. Тестирование
Проект покрыт модульными тестами, проверяющими как бизнес-правила домена, так и логику обработчиков команд.

```bash
cd LOS.Tests
dotnet test
```
---
### 👤 Автор
**Mikhail Mamonov** <br>
🔗 [GitHub Profile](https://github.com/MikhailMamonov) <br>
📧 Свяжитесь со мной для обсуждения деталей архитектуры или возможностей сотрудничества.
