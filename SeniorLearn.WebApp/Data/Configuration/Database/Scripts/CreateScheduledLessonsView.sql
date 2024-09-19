IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[timetable].[ViewScheduledLessons]'))
BEGIN
    DROP VIEW [timetable].[ViewScheduledLessons];
END
EXEC dbo.sp_executesql @statement =
N'CREATE VIEW [timetable].[ViewScheduledLessons]
AS
SELECT        
l.Id, 
l.[Name] Title, 
l.[Start], 
DATEADD(MINUTE,ClassDurationInMinutes,[Start]) [End],
l.[Description], 
ClassDurationInMinutes, 
TimeTableId, 
DeliveryPatternId, 
TopicId, 
t.[Name] Topic,
p.Id ProfessionalId, 
CONCAT(m.FirstName,'' '',m.LastName,  '' - '' , u.Email) Professor,
StatusId,
dp.IsCourse
FROM   
timetable.Lessons l
inner join timetable.DeliveryPatterns dp on dp.Id = l.DeliveryPatternId
inner join timetable.Topics t on t.Id = l.TopicId
inner join org.Professional p on p.Id = dp.ProfessionalId
inner join org.UserRolesTypes ur on p.Id = ur.Id
inner join org.AspNetUsers u on u.Id = ur.UserId
inner join org.Members m on m.UserId = u.Id
'

