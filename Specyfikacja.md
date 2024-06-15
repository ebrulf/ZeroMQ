# Do zrobienia z Systemów rozproszone

## Projekt zaliczeniowy

Czego chce: Gra sieciowa

> 1. Gra dla par graczy. Serwer pozwala klientom włączyć się do gry, tworzy osobny wątek/[wątki](https://e.sggw.pl/mod/folder/view.php?id=213921 "Wątki")/proces czy inny analog dla każdej pary i pozwala im grać.
> 
> 2. Gra typu MMORPG - oczywiście nie musi być grywalna. W każdym razie  mamy
>    serwer, wszyscy gracze komunikują się z nim z maszyn klienckich, serwer
>    zapisuje ich ruchy, sprawdza gdzie są, czy mogą wykonać dany ruch, itd.
>    Komunikacja z każdym z graczy powinna się odbywać w osobnym 
>    wątku/procesie/współprogramie - tak, jak dla wariantu pierwszego.

> Gra nie musi zawierać interfejsu graficznego czy okienkowego, ale może, a wszelkie ,,bajery'' mogą podnieść ocenę.  
> Mechanizm komunikacji: RabbitMQ jest bardzo mile widziane, ale może być także co innego: sockety UDP, sockety TCP, [XML-RPC](https://e.sggw.pl/mod/folder/view.php?id=213924 "XML-RPC"), websockets, itd.; przy czym programy używające nie zwykłych gniazdek, lecz czegoś ciekawszego będą oceniane wyżej.

> Język implementacji: Python (oczywiście preferowany, ale bynajmniej 
> nie obowiązkowy), C, C++, C#, ew. Java (a, fuj!) - wedle Waszego 
> uznania; inny język - po konsultacji ze mną i uzyskaniu mojej zgody.

> Uwaga!
>  Oczywiście, można korzystać z materiałów znalezionych w sieci - może to
>  być nawet dobry pomysł; znajdujemy w sieci jakąś grę: statki, szachy, 
> itd. - ale działająca lokalnie, na jednej maszynie i dorabiamy do niej 
> komunikację.przez sieć. W przypadku korzystania z czegokolwiek, czego 
> sami nie napisaliście, proszę jednak o *wyraźne* stwierdzenie tego w 
> komentarzu i podanie *źródła*!!!

Moje Założenie: Kółko i krzyżyk 3x3

Język programowania: C# 12 (.NET 8) (biblioteki: NetMQ 4.0.1.13, .NET MAUI)

Użyta technologia: ZeroMQ, protokół ZMTP 3.1 (nie wiem, jaka jest najnowsza wersja zaimplementowana w NetMQ)

Lokalizacja: w budowie

Platformy docelowe: Windows 10 2004 x64, bez ekranu dotykowego, język polski

### A teraz to wyjaśnij gumowej kaczuszce:

Mam zrobić grę sieciową. Kółko i krzyżyk to (obok młynka) jedna z najprostszych<sup>[<mark>potrzebne źródło</mark>]</sup> asynchronicznych gier dla dwóch osób grających. Prosta do zaimplementowania gra to obok prostej do ogarnięcia struktury sieciowej główna strategia ułatwienia sobie dokończenia pracy zaliczeniowej.

Ze specyfikacji wynika, że aplikacje klienckie wszelkie interakcje przeprowadzają poprzez serwer.

### Spodziewany scenariusz użytkowania:

(zakładam użycie zaimków on/jego)

Po odpaleniu aplikacji gracza wita ekran wstępny z formularzem jednokrotnego wyboru: gra lokalna albo wpisz adres (no i z przyciskiem na dole potwierdzającym wybór). W przypadku wyboru gry lokalnej program wyszuka automatycznie serwera spośród najbliższych 255 sąsiadów w podsieci aktywnego serwera, by się do niego podłączyć i zgłosić się jako chętny do gry. W przypadku wyboru drugiej opcji poszerzone pole do wpisania adresu IPv4 do podłączenia się do serwera.

W przypadku, gdy pod danym adresem nie ma dostępnego serwera, ma wyświetlić się monit z wiadomością "Nie odnaleziono serwera". W innym przypadku przełącza się na ekran gry i oczekuje na dołączenie drugiego gracza do rozpoczętego przez serwer wątku.

Ekran gry składa się z napisu wskazującego (też kolorem) kolej danego gracza i planszy do gry. Kliknięcie pola na planszy wiąże się z wykonaniem ruchu. Nie będzie można kliknąć w już zajęte pole.

Po skończonej grze można rozpocząć grę ponownie. Nie przewiduje się zapamiętywania wyników, remisów ani walkowerów. Walkower nie liczy się jako przegrana.

W przypadku przerwania połączenia między klientami, ma wyświetlić się po kilku sekundach monit z dwiema opcjami: ponownego odświeżenia i opuszczenia gry. Opuszczenie gry równa się oddaniu gry walkowerem oraz zamknięcia połączenia/wątku między graczami.

Da się zmienić rozmiar okna.

Nie jest planowana zmiana języka ani kolorów. Jasna kolorystyka. Nie planowana jest implementacja interfejsu według zaleceń CUA ani udogodnienia dla niepełnosprawnych.


