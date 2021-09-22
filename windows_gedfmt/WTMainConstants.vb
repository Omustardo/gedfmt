Module WTMainConstants
    'Line types
    'Currently 0 to 7 inclusive
    Public Const LINETYPE_CATEGORY As Integer = 7
    Public Const LINETYPE_EXTRACTED As Integer = 1
    Public Const LINETYPE_NORMAL As Integer = 0
    Public Const LINETYPE_ODM As Integer = 2
    Public Const LINETYPE_REFERENCE As Integer = 6
    Public Const LINETYPE_SOURCE As Integer = 5
    Public Const LINETYPE_TABLE As Integer = 4
    Public Const LINETYPE_UNSUPPORTED As Integer = 3
    'Note: This is NOT the section ID number in the class (the section index)
    'Currently 0 to 11 inclusive
    Public Const SECTION_ACKNOL As Integer = 8
    Public Const SECTION_BAPT As Integer = 2
    Public Const SECTION_BIRTH As Integer = 1
    Public Const SECTION_BURIAL As Integer = 11
    Public Const SECTION_CENSUS As Integer = 6
    Public Const SECTION_CHILDR As Integer = 5
    Public Const SECTION_DEATH As Integer = 10
    Public Const SECTION_DIVOR As Integer = 4
    Public Const SECTION_EVENTS As Integer = 99
    Public Const SECTION_FREESPACE As Integer = 0
    Public Const SECTION_GENERAL As Integer = 9
    Public Const SECTION_MARR As Integer = 3
    Public Const SECTION_NOTES As Integer = 7
    'Level 2 header types
    'In steps of 100: currently 100 to 500 inclusive
    Public Const L2HEADR_BIOG As Integer = 200
    Public Const L2HEADR_FREESPACE As Integer = 100
    Public Const L2HEADR_SRCES As Integer = 400
    Public Const L2HEADR_TIMEL As Integer = 300
    Public Const L2HEADR_UNKNOWN As Integer = 500
    'Date status indicators
    'Currently 0 to 4 inclusive
    Public Const DATE_ABT As Integer = 3
    Public Const DATE_AFT As Integer = 2
    Public Const DATE_BFR As Integer = 1
    Public Const DATE_CERT As Integer = 4
    Public Const DATE_UNK As Integer = 0
    'Styles
    Public Const BRACKET_NAME_TYPES As Integer = 1
    Public Const CREATE_EMPTY_PLACEHOLDERS As Integer = 2
    Public Const FS_BLOCK_COMPACT As Integer = 3
    Public Const CLEAR_ON_CLIPBOARD As Integer = 7
    Public Const LAYOUT_STYLE As Integer = 10
    Public Const TIMELINE_STYLE As Integer = 11
    Public Const DATES_FORMAT As Integer = 12
    Public Const PAD_SINGLE_DIGITS As Integer = 13
    Public Const ORDINAL_INDICATORS As Integer = 14
    Public Const CREATE_TEMPLATES As Integer = 15
    Public Const SHORT_MONTHS As Integer = 16
    Public Const DOUBLE_INDENT As Integer = 17
    Public Const RETAIN_IMPORT_INFO As Integer = 18
    Public Const RETAIN_NO_MORE_INFO As Integer = 19
    Public Const ADD_NO_MORE_INFO As Integer = 20
    Public Const ADD_THRESHOLD As Integer = 21
    'Census Dates CANADA
    'The 1861 census began on different dates depending on location:
    'For Canada East and Canada West, 14 January 1861
    Public Const CENSUS_DATE_1861CAEW As String = "14th January, 1861"
    'For Nova Scotia, 30 March 1861
    Public Const CENSUS_DATE_1861CANS As String = "30th March, 1861"
    'For New Brunswick, 15 August 1861
    Public Const CENSUS_DATE_1861CANB As String = "15th August, 1861"
    'For Prince Edward Island, it is not known
    Public Const CENSUS_DATE_1861CAPEI As String = "1861"
    '
    'Births and Christenings
    Public Const FSCOLLECTION_ENGBRTHCHR_1538_1975 As String = "&collection_id=1473014"
    Public Const FSCOLLNAME__MASUSABRTHCHR_1639_1915 As String = "Massachusetts Births and Christenings"
    Public Const FSCOLLID_MASUSABRTHCHR_1639_1915 As String = "&collection_id=1675197"
    'Marriages
    Public Const FSCOLLECTION_ENGMARR_1538_1973 As String = "&collection_id=1473015"
    'Deaths and burials
    Public Const FSCOLLECTION_ENFDEABUR_1538_1991 As String = "&collection_id=1473016"
    'Census Dates UNITED KINGDOM
    'The date of the 1841 census was the night of 6 June 1841.
    Public Const CENSUS_DATE_1841UK As String = "6th June, 1841"
    'The date of the 1851 census was the night of 30 March 1851.
    Public Const CENSUS_DATE_1851UK As String = "30th March, 1851"
    'The date of the 1861 census was the night of 7 April 1861.
    Public Const CENSUS_DATE_1861UK As String = "7th April, 1861"
    Public Const FSCOLLECTION_EWCENSUS_1861 As String = "&collection_id=1493747"
    'The date of the 1871 census was the night of 2 April 1871.
    Public Const CENSUS_DATE_1871UK As String = "2nd April, 1871"
    Public Const FSCOLLECTION_EWCENSUS_1871 As String = "&collection_id=1538354"
    'The date of the 1881 census was the night of 3 April 1881 .
    Public Const CENSUS_DATE_1881UK As String = "3rd April, 1881"
    Public Const FSCOLLECTION_EWCENSUS_1881 As String = "&collection_id=2562194"
    'The date of the 1891 census was the night of 5 April 1891.
    Public Const CENSUS_DATE_1891UK As String = "5th April, 1891"
    Public Const FSCOLLECTION_EWCENSUS_1891 As String = "&collection_id=1865747"
    'The date of the 1901 census was the night of 31 March 1901.
    Public Const CENSUS_DATE_1901UK As String = "31st March, 1901"
    Public Const FSCOLLECTION_EWCENSUS_1901 As String = "&collection_id=1888129"
    'The date of the 1911 census was the night of 2 April 1911.
    Public Const CENSUS_DATE_1911UK As String = "2nd April, 1911"
    Public Const FSCOLLECTION_EWCENSUS_1911 As String = "&collection_id=1921547"
    'Family Search base search address
    Public Const FAMILY_SEARCH_BASE As String = "https://familysearch.org/search/record/results?count=75"
End Module
