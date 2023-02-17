# ComputerServiceOIRPOS

Analiza
Aplikacja umożliwia rejestracje oraz logowanie poszczególnych użytkowników. Dostępne są następujące role:

Niezalogowany
Klient
Pracownik
Support
Manager
Admin


Niezalogowany - Przeglądanie cen, sprawdzenie stanu obecnej naprawy za pomocą specjalnego kodu.

Klient - Możliwość przeglądania historii napraw, sprawdzenie stanu obecnej naprawy.

Pracownik - Możliwość przyjmowania nowych zleceń naprawy, dodawanie nowych przedmiotów do bazy danych, dodawanie nowych osób do bazy danych.

Support - Pracownik odpowiedzialny za korespondencję z klientami.

Manager - Możliwość rejestrowania nowych pracowników, zmian w tabeli z usługami oraz ich cenami, generowanie raportów.

Admin - Możliwość dostępu do wszystkich funkcji programu oraz modyfikowania każdej z używanych tabel.
Specyfikacja wewnętrzna
W projekcie skorzystano z następujących technologii:

Entity framework
Baza danych SQLite
MVC
Autentykacja/Autoryzacja
Programowanie asynchroniczne


Specyfikacja zewnętrzna
Aplikacja roboczo jest hostowana na komputerze lokalnym. Po wpisaniu w pasek przeglądarki adresu https://localhost:45443 ukazuje nam się strona główna:



Domyślnie użytkownik jest niezalogowany. W tym stanie ma dostęp do następujących funkcji:

Sprawdzenie stanu naprawy na podstawie otrzymanego kodu odbioru(zakładka “Pickup codes”):

Możliwe wartości stanu naprawy to: przyjęta, w trakcie, gotowa, wydana.

Sprawdzenie cen w serwisie(zakładka “Prices”):



Możliwość logowania:

Rejestracja:


Użytkownik:
Po zalogowaniu użytkownik ma dostęp do panelu “My profile” gdzie może zmienić swoje dane osobowe:


Dodatkowo otrzymuje dostęp do panelu “My Repairs” w którym może sprawdzić stan swoich napraw:

Każdy zalogowany uzytkownik posiada również dostęp do wewnętrzej skrzynki pocztowej.
Wysyłanie wiadomości:


Skrzynka odbiorcza:

Podgląd wiadomości:

Skrzynka nadawcza:



Pracownik:
Konta pracownicze posiadają możliwość dodania nowego przedmiotu do bazy danych:


Aby dodać przedmiot konieczne jest podanie właściciela, w przypadku braku istniejącego konta, pracownik może założyć profil danej osoby:
Wybór osoby z listy zarejestrowanych

Aby rozpocząć naprawe pracownik musi skojarzyć naprawę z przedmiotem:

Wybór przedmiotu z listy:
Wybór daty przyjęcia do naprawy oraz pole informujące czy naprawa jest gwarancyjna:

Lista uslug wykonanych w ramach naprawy, pracaownik ma możliwość dodania kolejnych usług:
Dodawanie kolejnej usługi do naprawy:


Dla konta menedżera poza opcjami pracownika dostępne są dodatkowo możliwości:

Wykaz statystyk dot. pracy serwisu:





Dodanie nowej usługi do cennika:



Rejestracja nowego pracownika:



Dodatkowo dla konta administratora, poza wszystkimi powyższymi opcjami dostępna jest dodatkowa zakładka, która umożliwia zarządzanie wszystkimi użytkownikami dostępnymi w bazie(tj. zmianę uprawnień, rodzaju konta):

