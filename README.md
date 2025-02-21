
## CryptoPerCurrency - Returns crypto price per multiple currencies 

Just tried to keep it simple and stay away from the Programming OCD (POCD).
Quartz being used in the background to cache, improve performance and keeping the request limit low under heavy loads. 
simply it gets symbols from  [@CinMarketCap](https://coinmarketcap.com/) and selects the top 100 for the most frequent symbols.
You are free to use it as any type of service(micro,..) for your projects under MIT Licence.


[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)




## Routes&Defaults

| Route                | Result       |
|----------------------|--------------|
| /api/rate            | Default(BTC) |
| /api/rate?symbol=BTC | BTC          |
| /api/rate?symbol=ETH | ETH          |
| /api/rate?symbol=... | ...          |



## Attention

Please make sure to use your appropriate token from CoinMarketCap and make sure you not share it with others.



## Docker

To Dockerize the project using multi stage build.

```bash
  docker build .
```


## Run Locally

Clone the project

```bash
  git clone https://github.com/thisissoroush/`CryptoPerCurrency`
```

Go to the project directory

```bash
  cd https://github.com/thisissoroush/CryptoPerCurrency
```

Install dependencies

```bash
  dotnet restore
```

Build the project

```bash
  dotnet build
```

Start the server

```bash
  dotnet run
```


## Author

- [@Soroush Nasiri](https://www.github.com/Thisissoroush)

