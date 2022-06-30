# csharp-twse-stock-etf-list
[C#] 取得證交所上市及上櫃的股票及ETF清單

在證券交易所及櫃買中心市場上每一天都會有上市或下市的有價證券清單，做程式交易除了每日更新股價之外，還需要定期更新一下新上市或已下市的有價證券清單。
在證交所有一個網址可以查看最新上市或上櫃的清單。
### 查詢上市清單網址

https://isin.twse.com.tw/isin/C_public.jsp?strMode=2
![](https://blog.hungwin.com.tw/wp-content/uploads/2021/05/csharp-twse-stock-etf-list-2.jpg)

### 查詢上櫃清單網址

https://isin.twse.com.tw/isin/C_public.jsp?strMode=4
![](https://blog.hungwin.com.tw/wp-content/uploads/2021/05/csharp-twse-stock-etf-list-3.jpg)

透過這 2 個網址就可以知道最新的上市或上櫃有價證券清單是什麼。
可是這清單實在是太長了，包含了眾多的有價證券類別，如果只想看最常見的股票及 ETF 清單，
那就需要用程式把清單中只屬於股票及 ETF 的清單篩選出來。

接下來我會示範一下如何利用 C# 取得證交所有價證券清單的上市及上櫃股票 ETF 清單。

[完整文章連結](https://blog.hungwin.com.tw/csharp-twse-stock-etf-list/)
