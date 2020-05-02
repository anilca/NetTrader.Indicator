# NetTrader.Indicator
Technical analysis library for .NET

﻿Sample for Relative Strength Index (RSI) usage:

```C#
  RSI rsi = new RSI(14);
  List<Ohlc> ohlcList = new List<Ohlc>();
  // fill ohlcList
  rsi.Load(ohlcList);
  RSISerie serie = rsi.Calculate();
```

Library includes the following implementations:
- Accumulation/Distribution (ADL) 
- Average Directional Index (ADX)
- Aroon
- Average True Range (ATR)
- BollingerBand
- Commodity Channel Index (CCI)
- Chaikin Money Flow (CMF)
- Chande Momentum Oscillator (CMO)
- Double Exponential Moving Average (DEMA)
- Detrended Price Oscillator (DPO)
- Exponential Moving Average (EMA)
- Moving Average Envelopes (Envelope)
- Ichimoku Clouds (Ichimoku)
- Moving Average Convergence Divergence (MACD)
- Momentum
- On Balance Volume (OBV)
- Price Volume Trend (PVT)
- Rate of Change (ROC)
- Relative Strength Index (RSI)
- Stop and Reverse (SAR)
- Simple Moving Average (SMA)
- TRIX
- Volume
- Volume Rate of Change (VROC)
- Weighted Moving Average (WMA)
- Williams %R (WPR)
- Zero Lag EMA (ZLEMA)



