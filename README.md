# ComputerServiceOIRPOS

# Analiza

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

# Specyfikacja wewnętrzna
W projekcie skorzystano z następujących technologii:

Entity framework
Baza danych SQLite
MVC
Autentykacja/Autoryzacja
Programowanie asynchroniczne


# Specyfikacja zewnętrzna
Aplikacja roboczo jest hostowana na komputerze lokalnym. Po wpisaniu w pasek przeglądarki adresu https://localhost:45443 ukazuje nam się strona główna:



Domyślnie użytkownik jest niezalogowany. W tym stanie ma dostęp do następujących funkcji:

Sprawdzenie stanu naprawy na podstawie otrzymanego kodu odbioru(zakładka “Pickup codes”):
![image](https://user-images.githubusercontent.com/43991762/219813689-e6c8579c-9089-411e-8493-058ba236f6c1.png)


Możliwe wartości stanu naprawy to: przyjęta, w trakcie, gotowa, wydana.

Sprawdzenie cen w serwisie(zakładka “Prices”):
![image](https://user-images.githubusercontent.com/43991762/219813794-eb039822-de95-4fe1-a188-c6064b7bee1e.png)



Możliwość logowania:
![image](https://user-images.githubusercontent.com/43991762/219813814-e302536f-3de6-41d5-a66e-acb55cf1ab38.png)

Rejestracja:
![image](https://user-images.githubusercontent.com/43991762/219813834-211cade6-fe2a-4525-8b1b-293520f942d0.png)


Użytkownik:
Po zalogowaniu użytkownik ma dostęp do panelu “My profile” gdzie może zmienić swoje dane osobowe:
![image](https://user-images.githubusercontent.com/43991762/219813870-bee67774-fd38-4a2d-9599-ce8e447219a2.png)


Dodatkowo otrzymuje dostęp do panelu “My Repairs” w którym może sprawdzić stan swoich napraw:
![image](https://user-images.githubusercontent.com/43991762/219813884-823665d9-c8a3-467d-b193-7e3949741dae.png)

Każdy zalogowany uzytkownik posiada również dostęp do wewnętrzej skrzynki pocztowej.
Wysyłanie wiadomości:
![image](https://user-images.githubusercontent.com/43991762/219813905-0e46cc8e-2d28-41ec-8c7b-e31ea7bd7281.png)


Skrzynka odbiorcza:
![image](https://user-images.githubusercontent.com/43991762/219813930-800cb242-48e9-451b-8f7e-f606863678aa.png)

Podgląd wiadomości:
![image](https://user-images.githubusercontent.com/43991762/219813952-e43e35ef-f959-44d6-8665-68a0c125ddfc.png)

Skrzynka nadawcza:
![image](https://user-images.githubusercontent.com/43991762/219813979-74abcf64-5535-45a7-8f17-e74dc3d3d022.png)



Pracownik:
Konta pracownicze posiadają możliwość dodania nowego przedmiotu do bazy danych:
![image](https://user-images.githubusercontent.com/43991762/219814004-bba12543-258f-498c-85bf-41e110b5e5e7.png)


Aby dodać przedmiot konieczne jest podanie właściciela, w przypadku braku istniejącego konta, pracownik może założyć profil danej osoby:
![image](https://user-images.githubusercontent.com/43991762/219814039-1859837e-da75-47e3-bab1-a1d9cbf188e3.png)

Wybór osoby z listy zarejestrowanych
![image](https://user-images.githubusercontent.com/43991762/219814065-4f1ada27-ffc3-479a-8b35-8154884510c8.png)


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

