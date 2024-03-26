# MasterPlanner

Планировщик задач - приложение для удобного управления личными делами. С помощью встроенного календаря можно с легкостью создать расписание и отслеживать свои планы на день. Интерфейс настолько интуитивно понятен, что приложение подойдет даже для самых маленьких пользователей.

Функционал приложения включает в себя:
1. Возможность добавлять заметки.
2. Выбор даты или промежутка дат для каждой заметки.
3. Редактирование заметок.
4. Удаление заметок.
5. Добавление напоминаний, если это необходимо.

Инструкция по запуску:
1. Установить PostgreSQL.
2. В pgAdmin перейти Databases -> postgres -> Schemas -> public и выберите Tables.
3. Выберите Query Tool во вкладке Tools.
4. Введите в окно Query:
CREATE TABLE datenotes (
    id SERIAL PRIMARY KEY,
    date TIMESTAMP,
    date_end TIMESTAMP,
    notes TEXT,
    reminder_date TIMESTAMP,
    should_remind BOOLEAN,
    reminder_shown BOOLEAN
);
5. Затем клонируйте репозиторий из github: https://github.com/MrVoodoPeople/MasterPlanner.git (Напрямую, либо используя кнопку Open with Visual Studio во вкладке <>Code)
6. Загрузите предложенные библиотеки после запуска проекта в Visual Studio.
7. Теперь вам доступна отладка.

Команда:
Сергей "MrVoodoPeople" Слепнев - кодер
Иван "ZexisMemo" Федоров - кодер
Максим "Scumrock" Корепанов - менеджер