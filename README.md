# SkillProfi
 

---Проект SkillProfi (веб сайт):

Для сборки проекта требуется подключить к проекту NuGet пакеты:
Bootstrap версии 5.3.2;
Newtonsoft.Json версии 13.0.1;
Microsoft.AspNetCore.Identity.EntityFrameworkCore версии 8.0.0;
Microsoft.EntityFrameworkCore.SqlServer версии 8.0.0;
Microsoft.EntityFrameworkCore.Tools версии 8.0.0;

В файле приложения appsettings.json указывается путь подключения к БД MS SQL Server;
Логин = "admin" пароль = "admin"



---Проект SkillProfiApi (Web API сервис):

Для сборки проекта требуется подключить к проекту NuGet пакеты:
Microsoft.AspNetCore.Identity.EntityFrameworkCore версии 8.0.0;
Microsoft.EntityFrameworkCore.SqlServer версии 8.0.0;
Microsoft.EntityFrameworkCore.Tools версии 8.0.0;
Swashbuckle.AspNetCore версии 6.4.0;

В файле приложения appsettings.json указывается путь подключения к БД MS SQL Server;



---Проект SkillProfiWPF (приложение WPF):

Для сборки проекта требуется подключить к проекту NuGet пакеты:
Microsoft.AspNet.WebApi.Client версии 6.0.0;
Microsoft.AspNetCore.Authorization версии 8.0.7;
Newtonsoft.Json версии 13.0.1;





===Запуск SkillProfi

1) Запускаем Visual Studio

- Выбираем "Открыть проект или решение"
- В открывшемся окне находим и выбираем папку SkillProfi-main, открываем ее
- Далее выбираем SkillProfi.sln и нажимаем открыть

2) Установка пакетов NuGet
- В открывшемся Visual Studio находим "Обозреватель решений"
- Правой кнопкой нажимаем на Решение SkillProfi и нажимаем Управление пакетами NuGet для решения...
- В открывшемся окне нажимаем на Обзор
- В поле поиск пишем название необходимого пакета, пакеты описаны выше в разделе ---Проект SkillProfi (веб сайт):
- Поочерёдно устанавливаем все необходимые пакеты определённой версии

3) Запуск приложения
- В верхней панели находим зелёную треугольную кнопку, слева от этой кнопки есть маленький треугольник, указывающий вниз для выбора вариантов, находим её и жмём
- В выпадающем меню выбираем SkillProfi
- В верхней панели находим зелёную треугольную кнопку, справа от этой кнопки есть маленький треугольник, указывающий вниз для выбора опции запуска, находим её и жмём
- В выпадающем меню зелёной треугольной кнопки выбираем опцию запуска IIS Express
- Запускаем приложение нажав на зелёную треугольную кнопку
- Запустится ваш браузер с нашим приложением



===Запуск SkillProfiApi

1) Запускаем Visual Studio

- Выбираем "Открыть проект или решение"
- В открывшемся окне находим и выбираем папку SkillProfi-main, открываем ее
- Далее выбираем SkillProfi.sln и нажимаем открыть

2) Установка пакетов NuGet
- В открывшемся Visual Studio находим "Обозреватель решений"
- Правой кнопкой нажимаем на Решение SkillProfiApi и нажимаем Управление пакетами NuGet для решения...
- В открывшемся окне нажимаем на Обзор
- В поле поиск пишем название необходимого пакета, пакеты описаны выше в разделе ---Проект SkillProfiApi (Web API сервис):
- Поочерёдно устанавливаем все необходимые пакеты определённой версии

3) Запуск приложения
- В верхней панели находим зелёную треугольную кнопку, слева от этой кнопки есть маленький треугольник, указывающий вниз для выбора вариантов, находим её и жмём
- В выпадающем меню выбираем SkillProfiApi 
- В верхней панели находим зелёную треугольную кнопку, справа от этой кнопки есть маленький треугольник, указывающий вниз для выбора опции запуска, находим её и жмём
- В выпадающем меню зелёной треугольной кнопки выбираем опцию запуска IIS Express
- Запускаем приложение нажав на зелёную треугольную кнопку
- Запустится ваш браузер с нашим приложением



===Запуск SkillProfiWPF

1) Запускаем Visual Studio

- Выбираем "Открыть проект или решение"
- В открывшемся окне находим и выбираем папку SkillProfi-main, открываем ее
- Далее выбираем SkillProfi.sln и нажимаем открыть

2) Установка пакетов NuGet
- В открывшемся Visual Studio находим "Обозреватель решений"
- Правой кнопкой нажимаем на Решение SkillProfiWPF и нажимаем Управление пакетами NuGet для решения...
- В открывшемся окне нажимаем на Обзор
- В поле поиск пишем название необходимого пакета, пакеты описаны выше в разделе ---Проект SkillProfiWPF (приложение WPF):
- Поочерёдно устанавливаем все необходимые пакеты определённой версии

3) Запуск приложения
- В верхней панели находим зелёную треугольную кнопку, слева от этой кнопки есть маленький треугольник, указывающий вниз для выбора вариантов, находим её и жмём
- В выпадающем меню выбираем SkillProfiWPF 
- В верхней панели находим зелёную треугольную кнопку, справа от этой кнопки есть маленький треугольник, указывающий вниз для выбора опции запуска, находим её и жмём
- В выпадающем меню зелёной треугольной кнопки выбираем опцию запуска Пуск
- Запускаем приложение нажав на зелёную треугольную кнопку
- Запустится наше WPF приложение
