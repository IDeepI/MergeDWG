Установка:

		При запуске "MergeDWG.exe" без параметров есть возможность вбить пути к файлу lisp, шаблону dwg файла, 
		смещение по оси X и на сколько символов обрезать имя файла в конце. Либо можно отредактировать файл 
		"MergeDWG.exe.config"

		Открыть Autocad. В консоле набрать "appload", выбрать "автозагрузка", "добавить" и найти созданный файл 
		"UniteInOne.lsp". "SECURELOAD" поставить в "0" либо положить файл в "TRUSTEDPATHS"

		Необходимо создать ярлык ("MergeDWG.lnk") на файл "..\source\repos\MergeDWG\MergeDWG\bin\Release\MergeDWG.exe" 
		в папке "c:\Users\UserName\AppData\Roaming\Microsoft\Windows\SendTo\"

Использование:

		Выделить файлы для объединения, нажать правой кнопкой, "Отправить", "MergeDWG.lnk"

		В открывшемся Autocad ввести в консоль "ii" 
