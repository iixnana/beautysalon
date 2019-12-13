˚	
HC:\Users\Kamile\source\repos\BeautySalon\Contracts\IArticleRepository.cs
	namespace 	
	Contracts
 
{ 
public 

	interface 
IArticleRepository '
:( )
IRepositoryBase* 9
<9 :
Article: A
>A B
{		 
IEnumerable

 
<

 
Article

 
>

 
GetAllArticles

 +
(

+ ,
)

, -
;

- .
IEnumerable 
< 
Article 
> 
GetArticlesByMaster 0
(0 1
int1 4
masterId5 =
)= >
;> ?
Article 
GetArticleById 
( 
int "
	articleId# ,
), -
;- .
void 
CreateArticle 
( 
Article "
article# *
)* +
;+ ,
void 
UpdateArticle 
( 
Article "
	dbArticle# ,
,, -
Article. 5
article6 =
)= >
;> ?
void 
DeleteArticle 
( 
Article "
article# *
)* +
;+ ,
} 
} ¥
BC:\Users\Kamile\source\repos\BeautySalon\Contracts\IAuthService.cs
	namespace 	
	Contracts
 
{ 
public		 

	interface		 
IAuthService		 !
{

 
bool 
IsAuthenticated 
( 
AuthData %
request& -
,- .
out/ 2
string3 9
token: ?
)? @
;@ A
JwtSecurityToken 
	ReadToken "
(" #
string# )
header* 0
)0 1
;1 2
bool 
IsAdmin 
( 
string 
header "
)" #
;# $
bool 
IsClient 
( 
string 
header #
)# $
;$ %
bool 
IsMaster 
( 
string 
header #
)# $
;$ %
int 
GetId 
( 
string 
header 
)  
;  !
} 
} Á
DC:\Users\Kamile\source\repos\BeautySalon\Contracts\ILoggerManager.cs
	namespace 	
	Contracts
 
{ 
public 

	interface 
ILoggerManager #
{ 
void		 
LogInfo		 
(		 
string		 
message		 #
)		# $
;		$ %
void

 
LogWarn

 
(

 
string

 
message

 #
)

# $
;

$ %
void 
LogDebug 
( 
string 
message $
)$ %
;% &
void 
LogError 
( 
string 
message $
)$ %
;% &
} 
} ∆
EC:\Users\Kamile\source\repos\BeautySalon\Contracts\IRepositoryBase.cs
	namespace 	
	Contracts
 
{ 
public		 

	interface		 
IRepositoryBase		 $
<		$ %
T		% &
>		& '
{

 

IQueryable 
< 
T 
> 
FindAll 
( 
) 
;  

IQueryable 
< 
T 
> 
FindByCondition %
(% &

Expression& 0
<0 1
Func1 5
<5 6
T6 7
,7 8
bool9 =
>= >
>> ?

expression@ J
)J K
;K L
void 
Create 
( 
T 
entity 
) 
; 
void 
Update 
( 
T 
entity 
) 
; 
void 
Delete 
( 
T 
entity 
) 
; 
} 
} è
HC:\Users\Kamile\source\repos\BeautySalon\Contracts\IRepositoryWrapper.cs
	namespace 	
	Contracts
 
{ 
public 

	interface 
IRepositoryWrapper '
{ 
IUserRepository		 
User		 
{		 
get		 "
;		" #
}		$ % 
ITimetableRepository

 
	Timetable

 &
{

' (
get

) ,
;

, -
}

. /
IArticleRepository 
Article "
{# $
get% (
;( )
}* +
IServiceRepository 
Service "
{# $
get% (
;( )
}* +"
IReservationRepository 
Reservation *
{+ ,
get- 0
;0 1
}2 3
void 
Save 
( 
) 
; 
} 
} ì
WC:\Users\Kamile\source\repos\BeautySalon\Contracts\IReservationHasServicesRepository.cs
	namespace 	
	Contracts
 
{ 
public 

class -
!IReservationHasServicesRepository 2
{		 
} 
} Ï
LC:\Users\Kamile\source\repos\BeautySalon\Contracts\IReservationRepository.cs
	namespace 	
	Contracts
 
{ 
public		 

	interface		 "
IReservationRepository		 +
:		, -
IRepositoryBase		. =
<		= >
Reservation		> I
>		I J
{

 
IEnumerable 
< 
Reservation 
>  !
GetReservationsByUser! 6
(6 7
int7 :
userId; A
)A B
;B C
IEnumerable 
< 
Reservation 
>  #
ReservationsByTimetable! 8
(8 9
DateTime9 A
dateB F
,F G
TimeSpanH P
startQ V
,V W
TimeSpanX `
enda d
)d e
;e f
IEnumerable 
< 
Reservation 
>  
GetAllReservations! 3
(3 4
)4 5
;5 6
IEnumerable 
< 
Reservation 
>  #
GetReservationsByMaster! 8
(8 9
int9 <
masterId= E
)E F
;F G
IEnumerable 
< 
Reservation 
>  *
GetReservationsByMasterAndDate! ?
(? @
int@ C
masterIdD L
,L M
DateTimeN V
dateW [
)[ \
;\ ]
Reservation 
GetReservationById &
(& '
int' *
userId+ 1
)1 2
;2 3
bool 
IsValid 
( 
int 
masterId !
,! "
DateTime# +
date, 0
,0 1
TimeSpan2 :
start; @
,@ A
TimeSpanB J
endK N
)N O
;O P
void 
CreateReservation 
( 
Reservation *
reservation+ 6
)6 7
;7 8
void 
UpdateReservation 
( 
Reservation *
dbReservation+ 8
,8 9
Reservation: E
reservationF Q
)Q R
;R S
void 
DeleteReservation 
( 
Reservation *
reservation+ 6
)6 7
;7 8
} 
} æ
HC:\Users\Kamile\source\repos\BeautySalon\Contracts\IServiceRepository.cs
	namespace 	
	Contracts
 
{ 
public 

	interface 
IServiceRepository '
:( )
IRepositoryBase) 8
<8 9
Service9 @
>@ A
{		 
IEnumerable

 
<

 
Service

 
>

 !
ServicesByReservation

 2
(

2 3
int

3 6
masterId

7 ?
)

? @
;

@ A
IEnumerable 
< 
Service 
> 
GetAllServices +
(+ ,
), -
;- .
Service 
GetServiceById 
( 
int "
	serviceId# ,
), -
;- .
IEnumerable 
< 
Service 
> 
GetServicesByMaster 0
(0 1
int1 4
masterId5 =
)= >
;> ?
void 
CreateService 
( 
Service "
service# *
)* +
;+ ,
void 
UpdateService 
( 
Service "
	dbService# ,
,, -
Service. 5
service6 =
)= >
;> ?
void 
DeleteService 
( 
Service "
service# *
)* +
;+ ,
} 
} Á
JC:\Users\Kamile\source\repos\BeautySalon\Contracts\ITimetableRepository.cs
	namespace 	
	Contracts
 
{ 
public 

	interface  
ITimetableRepository )
:* +
IRepositoryBase, ;
<; <
	Timetable< E
>E F
{		 
IEnumerable

 
<

 
	Timetable

 
>

 
GetAllTimetables

 /
(

/ 0
)

0 1
;

1 2
	Timetable 
GetTimetableById "
(" #
int# &
timetableId' 2
)2 3
;3 4
IEnumerable 
< 
	Timetable 
> !
GetTimetablesByMaster 4
(4 5
int5 8
masterId9 A
)A B
;B C
IEnumerable 
< 
	Timetable 
> (
GetTimetablesByMasterAndDate ;
(; <
int< ?
masterId@ H
,H I
DateTimeJ R
dateS W
)W X
;X Y
	Timetable +
GetTimetableByMasterAndDateTime 1
(1 2
int2 5
masterId6 >
,> ?
DateTime@ H
dateI M
,M N
TimeSpanO W
startX ]
,] ^
TimeSpan_ g
endh k
)k l
;l m
bool 
IsValid 
( 
	Timetable 
	timetable (
)( )
;) *
void 
CreateTimetable 
( 
	Timetable &
	timetable' 0
)0 1
;1 2
void 
UpdateTimetable 
( 
	Timetable &
dbTimetable' 2
,2 3
	Timetable4 =
	timetable> G
)G H
;H I
void 
DeleteTimetable 
( 
	Timetable &
	timetable' 0
)0 1
;1 2
} 
} ˘
EC:\Users\Kamile\source\repos\BeautySalon\Contracts\IUserRepository.cs
	namespace 	
	Contracts
 
{ 
public		 

	interface		 
IUserRepository		 $
:		% &
IRepositoryBase		' 6
<		6 7
User		7 ;
>		; <
{

 
IEnumerable 
< 
User 
> 
GetAllUsers %
(% &
)& '
;' (
User 
GetUserById 
( 
int 
userId #
)# $
;$ %
User 
GetUserByEmail 
( 
string "
email# (
)( )
;) *
IEnumerable 
< 
User 
> 
GetAllMasters '
(' (
)( )
;) *
IEnumerable 
< 
User 
> 
GetAllClients '
(' (
)( )
;) *
IEnumerable 
< 
User 
> 
GetAllAdmins &
(& '
)' (
;( )
UserExtended 
GetUserWithDetails '
(' (
int( +
userId, 2
)2 3
;3 4
void 

CreateUser 
( 
User 
user !
)! "
;" #
void 

UpdateUser 
( 
User 
dbUser #
,# $
User% )
user* .
). /
;/ 0
void 

DeleteUser 
( 
User 
user !
)! "
;" #
bool 
IsValidUser 
( 
string 
email  %
,% &
string' -
password. 6
)6 7
;7 8
} 
} 