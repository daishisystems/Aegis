SELECT DISTINCT(IPAddress) FROM dbo.StreamingAnalyticsOutput;

SELECT TOP 1000 * FROM dbo.StreamingAnalyticsOutput;

SELECT * FROM BlackList where IPAddress = '185.24.235.177';

SELECT o.IPAddress FROM StreamingAnalyticsOutput o
LEFT JOIN BlackList b on o.IPAddress = b.IPAddress
WHERE b.IPAddress IS NULL;

SELECT * FROM StreamingAnalyticsOutput WHERE IPAddress = '46.105.144.18';

SELECT COUNT(*) FROM BlackList;

SELECT COUNT(DISTINCT(IPAddress)) FROM StreamingAnalyticsOutput;

SELECT IPAddress, MAX(ServerDateTime) AS LatestServerTime, SUM(Total) AS NumHits
FROM dbo.StreamingAnalyticsOutput
GROUP BY IPAddress;

SELECT IPAddress
FROM dbo.StreamingAnalyticsOutput
GROUP BY IPAddress;

SELECT * FROM dbo.StreamingAnalyticsOutput
WHERE IPAddress in ('109.76.143.45', '109.255.96.137', '109.77.100.110');

SELECT * FROM dbo.StreamingAnalyticsOutput
WHERE Total <= 40;

SELECT COUNT(*) FROM StreamingAnalyticsOutput;

TRUNCATE TABLE StreamingAnalyticsOutput;

SELECT * FROM StreamingAnalyticsOutput
WHERE IPAddress = '185.24.235.177'; /* ServeByte */

SELECT * FROM StreamingAnalyticsOutput
WHERE IPAddress = '178.167.254.145'; /* My phone */

SELECT * FROM StreamingAnalyticsOutput
WHERE IPAddress = '86.45.99.229'; /* Eircom */

SELECT * FROM StreamingAnalyticsOutput
WHERE IPAddress = '89.101.1.90'; /* AWS */

SELECT * FROM StreamingAnalyticsOutput
WHERE IPAddress = '45.55.239.101'; /* AWS */

SELECT IPAddress, COUNT(IPAddress) AS HyperActivity, SUM(Total) as TotalNumHits, SUM(Total)/COUNT(IPAddress) AS AVGNumHits
FROM StreamingAnalyticsOutput
WHERE IPAddress IN (
'185.24.235.177',
'52.19.63.190',
'52.17.234.55',
'213.233.147.83',
'104.132.8.111',
'109.77.150.22',
'178.167.254.220',
'178.167.254.80',
'37.228.227.99',
'93.95.86.14',
'37.228.206.36',
'89.100.79.236',
'178.167.254.184',
'86.46.62.132',
'86.44.188.20',
'86.45.99.229',
'109.77.177.164',
'78.19.163.123',
'109.76.180.19',
'109.125.16.171',
'78.16.111.81',
'109.78.148.242',
'95.44.129.104',
'86.44.204.246',
'78.18.109.136',
'176.61.1.12',
'31.187.36.5',
'213.233.150.20',
'87.232.34.187',
'213.233.150.19',
'37.228.225.51',
'80.111.188.15',
'78.17.122.130',
'109.79.60.40',
'89.100.18.187',
'92.251.255.12',
'213.233.132.183',
'86.45.236.253',
'109.78.59.228',
'86.40.189.123',
'83.71.193.244',
'178.167.254.102',
'95.44.130.245',
'217.114.169.241',
'95.83.254.224',
'78.17.247.120',
'86.41.200.186',
'213.233.147.161',
'178.167.254.221',
'109.125.19.205',
'46.7.14.203',
'213.233.132.136',
'109.125.18.106',
'92.235.37.222',
'37.228.230.28',
'178.167.254.133',
'213.233.132.177',
'95.45.46.224',
'78.18.13.161',
'89.100.62.138',
'78.18.170.202',
'213.233.150.123',
'52.16.237.132',
'109.78.1.8'
)
AND ServerDateTime >= GETDATE() - 1
GROUP BY IPAddress
ORDER BY TotalNumHits;

SELECT GETDATE() -1;

SELECT IPAddress, COUNT(IPAddress) AS HyperActivity, SUM(Total) as TotalNumHits, SUM(Total)/COUNT(IPAddress) AS AVGNumHits
FROM StreamingAnalyticsOutput
GROUP BY IPAddress
ORDER BY SUM(Total) DESC;

SELECT * FROM StreamingAnalyticsOutput
WHERE Total >= 60;

SELECT * FROM	(
	
	SELECT 
		IPAddress, 
		COUNT(IPAddress) AS HyperActivity, 
		SUM(Total) AS TotalNumHits,
		SUM(Total)/COUNT(IPAddress) AS AVGNumHits, 
		MAX(ServerDateTime) AS LatestServerTime
	FROM dbo.BlackList
	WHERE ServerDateTime >= GETDATE() - 1 /* 1st call - last 24 hours. Subsequent calls - since previous */
	GROUP BY IPAddress

				) AS BlackList

WHERE HyperActivity > 1
AND AVGNumHits >= 60
AND IPAddress = '213.233.132.177'
ORDER BY TotalNumHits DESC;

SELECT '[' + LowerIPAddress + ']', '[' + UpperIPAddress + ']'
FROM dbo.WhiteList;

-----
/* Ensure that whitelist ranges are correct in format */

SELECT COUNT(*)
FROM dbo.WhiteList
WHERE LowerIPAddress LIKE '% '
OR LowerIPAddress LIKE ' %'
OR UpperIPAddress LIKE '% '
OR UpperIPAddress LIKE ' %';

SELECT * FROM dbo.WhiteList
WHERE LowerIPAddress LIKE '%213%';

INSERT dbo.BlackList (IPAddress, [Path], Total)
VALUES ('213.233.132.177', 'TEST', 1000);

DELETE FROM dbo.BlackList
WHERE IPAddress = '213.233.132.177'
AND Total != 30;

SELECT TOP 10 * FROM DBO.BLACKLIST;

SELECT * FROM DBO.BlackList
WHERE IPAddress = '213.233.132.177';

UPDATE DBO.BlackList SET
	PATH = '/en-gb/availability'
WHERE IPAddress = '213.233.132.177';

SELECT * FROM dbo.blacklist
WHERE IPAddress = '213.233.132.177';


