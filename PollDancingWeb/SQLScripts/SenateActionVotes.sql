WITH XMLVotes AS (
    SELECT
        CONVERT(XML, REPLACE(RecordedVotes, '<?xml version="1.0" encoding="UTF-8"?>', ''), 2) AS VoteDataXML,
        LegislationId,
        ActionDate,
        ActionCode
    FROM
        dbo.Actions
    WHERE
        ActionDate = (
            SELECT MAX(ActionDate) 
            FROM dbo.Actions A2 
            WHERE A2.LegislationId = dbo.Actions.LegislationId AND A2.RecordedVotes IS NOT NULL
        ) AND ActionCode IN ('18000', '17000')
        AND IsParsed = 0
)

INSERT INTO MemberLegislationVotes (LegislationId, MemberId, Vote)
SELECT
    xVotes.LegislationId as LegislationId, 
    M.Id as MemberId,
    x.Rec.value('(vote_cast/text())[1]', 'varchar(10)') AS Vote
FROM
    XMLVotes AS xVotes
CROSS APPLY
    xVotes.VoteDataXML.nodes('/roll_call_vote/members/member') AS x(Rec)
JOIN 
    Members AS M 
    ON CHARINDEX(x.Rec.value('(last_name/text())[1]', 'varchar(100)'), M.Name) > 0
		AND CHARINDEX(x.Rec.value('(first_name/text())[1]', 'varchar(100)'), M.Name) > 0
WHERE NOT EXISTS (
    SELECT 1
    FROM MemberLegislationVotes MLV
    WHERE MLV.LegislationId = xVotes.LegislationId 
      AND MLV.MemberId = M.Id 
)
