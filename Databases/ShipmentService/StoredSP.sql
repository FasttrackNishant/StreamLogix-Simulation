select * from Shipments

select s.Id , ast.Name as [Asset_Name] ,ast.code , c.Name , st.Name ,  from Shipments S 
INNER JOIN
Assets ast on s.AssetId = ast.Id
inner join Addresses as addr
on s.SourceAddressId = addr.id
inner join Cities as c
on addr.CityId = c.Id
inner join states st
on c.StateId = st.Id
and s.DestinationAddressId = addr.Id and 




SELECT 
    s.Id AS ShipmentId,
    ast.Name AS AssetName,
    ast.Code AS AssetCode,

    -- Source Location
    srcCity.Name AS SourceCity,
    srcState.Name AS SourceState,

    -- Destination Location
    destCity.Name AS DestinationCity,
    destState.Name AS DestinationState,

    -- Source System
    ss.Name AS SourceSystemName,

    -- Shipment Info
    s.ExpectedDelivery,
    
    -- Warehouse Info
    wh.Name AS WarehouseName

FROM Shipments s

-- Asset Info
INNER JOIN Assets ast ON s.AssetId = ast.Id

-- Source Address → City → State
INNER JOIN Addresses srcAddr ON s.SourceAddressId = srcAddr.Id
INNER JOIN Cities srcCity ON srcAddr.CityId = srcCity.Id
INNER JOIN States srcState ON srcCity.StateId = srcState.Id

-- Destination Address → City → State
INNER JOIN Addresses destAddr ON s.DestinationAddressId = destAddr.Id
INNER JOIN Cities destCity ON destAddr.CityId = destCity.Id
INNER JOIN States destState ON destCity.StateId = destState.Id

-- Source System
LEFT JOIN SourceSystems ss ON s.SourceSystemId = ss.Id

-- Warehouse (via Asset)
LEFT JOIN Warehouses wh ON ast.WarehouseId = wh.Id;