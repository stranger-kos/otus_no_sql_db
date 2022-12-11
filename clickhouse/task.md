# Clickhouse

## Разворачивание БД
**Создание контейнера ubuntu** \
docker run -i -d ubuntu

**Вход в контейнер** \
docker exec -it 6668c7f5d636 bash

**Установка Clickhouse** \
sudo apt-get install apt-transport-https ca-certificates dirmngr && sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv E0C56BD4 && echo "deb https://repo.clickhouse.com/deb/stable/ main/" | sudo tee  /etc/apt/sources.list.d/clickhouse.list && sudo apt-get update && sudo apt-get install -y clickhouse-server clickhouse-client pv

**Скачивание тестовых данных** \
curl https://datasets.clickhouse.com/visits/tsv/visits_v1.tsv.xz | unxz --threads=`nproc` > visits_v1.tsv \
  % Total    % Received % Xferd  Average Speed   Time    Time     Time  Current\
                                 Dload  Upload   Total   Spent    Left  Speed\
100  405M  100  405M    0     0  2363k      0  0:02:55  0:02:55 --:--:-- 1460k

**Создание таблицы БД** \
CREATE TABLE tutorial.visits_v1\
(\
    &emsp;`CounterID` UInt32,\
    &emsp;`StartDate` Date,\
    &emsp;`Sign` Int8,\
    &emsp;`IsNew` UInt8,\
    &emsp;`VisitID` UInt64,\
    &emsp;`UserID` UInt64,\
    &emsp;`StartTime` DateTime,\
    &emsp;`Duration` UInt32,\
    &emsp;`UTCStartTime` DateTime,\
    &emsp;`PageViews` Int32,\
    &emsp;`Hits` Int32,\
    &emsp;`IsBounce` UInt8,\
    &emsp;`Referer` String,\
    &emsp;`StartURL` String,\
    &emsp;`RefererDomain` String,\
    &emsp;`StartURLDomain` String,\
    &emsp;`EndURL` String,\
    &emsp;`LinkURL` String,\
    &emsp;`IsDownload` UInt8,\
    &emsp;`TraficSourceID` Int8,\
    &emsp;`SearchEngineID` UInt16,\
    &emsp;`SearchPhrase` String,\
    &emsp;`AdvEngineID` UInt8,\
    &emsp;`PlaceID` Int32,\
    &emsp;`RefererCategories` Array(UInt16),\
    &emsp;`URLCategories` Array(UInt16),\
    &emsp;`URLRegions` Array(UInt32),\
    &emsp;`RefererRegions` Array(UInt32),\
    &emsp;`IsYandex` UInt8,\
    &emsp;`GoalReachesDepth` Int32,\
    &emsp;`GoalReachesURL` Int32,\
    &emsp;`GoalReachesAny` Int32,\
    &emsp;`SocialSourceNetworkID` UInt8,\
    &emsp;`SocialSourcePage` String,\
    &emsp;`MobilePhoneModel` String,\
    &emsp;`ClientEventTime` DateTime,\
    &emsp;`RegionID` UInt32,\
    &emsp;`ClientIP` UInt32,\
    &emsp;`ClientIP6` FixedString(16),\
    &emsp;`RemoteIP` UInt32,\
    &emsp;`RemoteIP6` FixedString(16),\
    &emsp;`IPNetworkID` UInt32,\
    &emsp;`SilverlightVersion3` UInt32,\
    &emsp;`CodeVersion` UInt32,\
    &emsp;`ResolutionWidth` UInt16,\
    &emsp;`ResolutionHeight` UInt16,\
    &emsp;`UserAgentMajor` UInt16,\
    &emsp;`UserAgentMinor` UInt16,\
    &emsp;`WindowClientWidth` UInt16,\
    &emsp;`WindowClientHeight` UInt16,\
    &emsp;`SilverlightVersion2` UInt8,\
    &emsp;`SilverlightVersion4` UInt16,\
    &emsp;`FlashVersion3` UInt16,\
    &emsp;`FlashVersion4` UInt16,\
    &emsp;`ClientTimeZone` Int16,\
    &emsp;`OS` UInt8,\
    &emsp;`UserAgent` UInt8,\
    &emsp;`ResolutionDepth` UInt8,\
    &emsp;`FlashMajor` UInt8,\
    &emsp;`FlashMinor` UInt8,\
    &emsp;`NetMajor` UInt8,\
    &emsp;`NetMinor` UInt8,\
    &emsp;`MobilePhone` UInt8,\
    &emsp;`SilverlightVersion1` UInt8,\
    &emsp;`Age` UInt8,\
    &emsp;`Sex` UInt8,\
    &emsp;`Income` UInt8,\
    &emsp;`JavaEnable` UInt8,\
    &emsp;`CookieEnable` UInt8,\
    &emsp;`JavascriptEnable` UInt8,\
    &emsp;`IsMobile` UInt8,\
    &emsp;`BrowserLanguage` UInt16,\
    &emsp;`BrowserCountry` UInt16,\
    &emsp;`Interests` UInt16,\
    &emsp;`Robotness` UInt8,\
    &emsp;`GeneralInterests` Array(UInt16),\
    &emsp;`Params` Array(String),\
    &emsp;`Goals` Nested(\
        &emsp;&emsp;ID UInt32,\
        &emsp;&emsp;Serial UInt32,\
        &emsp;&emsp;EventTime DateTime,\
        &emsp;&emsp;Price Int64,\
        &emsp;&emsp;OrderID String,\
        &emsp;&emsp;CurrencyID UInt32),\
    &emsp;`WatchIDs` Array(UInt64),\
    &emsp;`ParamSumPrice` Int64,\
    &emsp;`ParamCurrency` FixedString(3),\
    &emsp;`ParamCurrencyID` UInt16,\
    &emsp;`ClickLogID` UInt64,\
    &emsp;`ClickEventID` Int32,\
    &emsp;`ClickGoodEvent` Int32,\
    &emsp;`ClickEventTime` DateTime,\
    &emsp;`ClickPriorityID` Int32,\
    &emsp;`ClickPhraseID` Int32,\
    &emsp;`ClickPageID` Int32,\
    &emsp;`ClickPlaceID` Int32,\
    &emsp;`ClickTypeID` Int32,\
    &emsp;`ClickResourceID` Int32,\
    &emsp;`ClickCost` UInt32,\
    &emsp;`ClickClientIP` UInt32,\
    &emsp;`ClickDomainID` UInt32,\
    &emsp;`ClickURL` String,\
    &emsp;`ClickAttempt` UInt8,\
    &emsp;`ClickOrderID` UInt32,\
    &emsp;`ClickBannerID` UInt32,\
    &emsp;`ClickMarketCategoryID` UInt32,\
    &emsp;`ClickMarketPP` UInt32,\
    &emsp;`ClickMarketCategoryName` String,\
    &emsp;`ClickMarketPPName` String,\
    &emsp;`ClickAWAPSCampaignName` String,\
    &emsp;`ClickPageName` String,\
    &emsp;`ClickTargetType` UInt16,\
    &emsp;`ClickTargetPhraseID` UInt64,\
    &emsp;`ClickContextType` UInt8,\
    &emsp;`ClickSelectType` Int8,\
    &emsp;`ClickOptions` String,\
    &emsp;`ClickGroupBannerID` Int32,\
    &emsp;`OpenstatServiceName` String,\
    &emsp;`OpenstatCampaignID` String,\
    &emsp;`OpenstatAdID` String,\
    &emsp;`OpenstatSourceID` String,\
    &emsp;`UTMSource` String,\
    &emsp;`UTMMedium` String,\
    &emsp;`UTMCampaign` String,\
    &emsp;`UTMContent` String,\
    &emsp;`UTMTerm` String,\
    &emsp;`FromTag` String,\
    &emsp;`HasGCLID` UInt8,\
    &emsp;`FirstVisit` DateTime,\
    &emsp;`PredLastVisit` Date,\
    &emsp;`LastVisit` Date,\
    &emsp;`TotalVisits` UInt32,\
    &emsp;`TraficSource` Nested(\
        &emsp;&emsp;ID Int8,\
        &emsp;&emsp;SearchEngineID UInt16,\
        &emsp;&emsp;AdvEngineID UInt8,\
        &emsp;&emsp;PlaceID UInt16,\
        &emsp;&emsp;SocialSourceNetworkID UInt8,\
        &emsp;&emsp;Domain String,\
        &emsp;&emsp;SearchPhrase String,\
        &emsp;&emsp;SocialSourcePage String),\
    &emsp;`Attendance` FixedString(16),\
    &emsp;`CLID` UInt32,\
    &emsp;`YCLID` UInt64,\
    &emsp;`NormalizedRefererHash` UInt64,\
    &emsp;`SearchPhraseHash` UInt64,\
    &emsp;`RefererDomainHash` UInt64,\
    &emsp;`NormalizedStartURLHash` UInt64,\
    &emsp;`StartURLDomainHash` UInt64,\
    &emsp;`NormalizedEndURLHash` UInt64,\
    &emsp;`TopLevelDomain` UInt64,\
    &emsp;`URLScheme` UInt64,\
    &emsp;`OpenstatServiceNameHash` UInt64,\
    &emsp;`OpenstatCampaignIDHash` UInt64,\
    &emsp;`OpenstatAdIDHash` UInt64,\
    &emsp;`OpenstatSourceIDHash` UInt64,\
    &emsp;`UTMSourceHash` UInt64,\
    &emsp;`UTMMediumHash` UInt64,\
    &emsp;`UTMCampaignHash` UInt64,\
    &emsp;`UTMContentHash` UInt64,\
    &emsp;`UTMTermHash` UInt64,\
    &emsp;`FromHash` UInt64,\
    &emsp;`WebVisorEnabled` UInt8,\
    &emsp;`WebVisorActivity` UInt32,\
    &emsp;`ParsedParams` Nested(\
        &emsp;&emsp;Key1 String,\
        &emsp;&emsp;Key2 String,\
        &emsp;&emsp;Key3 String,\
        &emsp;&emsp;Key4 String,\
        &emsp;&emsp;Key5 String,\
        &emsp;&emsp;ValueDouble Float64),\
    &emsp;`Market` Nested(\
        &emsp;&emsp;Type UInt8,\
        &emsp;&emsp;GoalID UInt32,\
        &emsp;&emsp;OrderID String,\
        &emsp;&emsp;OrderPrice Int64,\
        &emsp;&emsp;PP UInt32,\
        &emsp;&emsp;DirectPlaceID UInt32,\
        &emsp;&emsp;DirectOrderID UInt32,\
        &emsp;&emsp;DirectBannerID UInt32,\
        &emsp;&emsp;GoodID String,\
        &emsp;&emsp;GoodName String,\
        &emsp;&emsp;GoodQuantity Int32,\
        &emsp;&emsp;GoodPrice Int64),\
    &emsp;`IslandID` FixedString(16)\
)\
ENGINE = CollapsingMergeTree(Sign)\
PARTITION BY toYYYYMM(StartDate)\
ORDER BY (CounterID, StartDate, intHash32(UserID), VisitID)\
SAMPLE BY intHash32(UserID)\
Query id: 9417ea22-154e-4fcf-9527-a9e59a192090\

Ok.

0 rows in set. Elapsed: 0.095 sec.