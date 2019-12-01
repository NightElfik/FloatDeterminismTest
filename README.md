This code simulates Sinai's billiards to test determinism of floating point operation in C#.

Results on my machine:

```
OS Name:                   Microsoft Windows 10 Pro
OS Version:                10.0.17134 N/A Build 17134
System Type:               x64-based PC
Processor(s):              1 Processor(s) Installed.
                           [01]: Intel64 Family 6 Model 94 Stepping 3 GenuineIntel ~4008 Mhz
```


## Test using NUnit Debug:

```
10
Without casts:            2,    0.2251443,     1.938195 (0x40000001, 0x3E668C38, 0x3FF816CA)
With casts:               2,    0.2251351,     1.938197 (0x40000000, 0x3E6689D2, 0x3FF816D9)
MISMATCH!!!

100
Without casts:    -1.119223,   -0.2299059,            2 (0xBF8F42AF, 0xBE6B6C76, 0x40000000)
With casts:               2,   -0.5220664,    0.4623849 (0x40000000, 0xBF05A624, 0x3EECBDB6)
MISMATCH!!!

1000
Without casts:    0.4805907,           -2,      1.84443 (0x3EF60FFC, 0xC0000000, 0x3FEC1648)
With casts:              -2,     -1.21301,   -0.4805112 (0xC0000000, 0xBF9B43E6, 0xBEF60590)
MISMATCH!!!

10000
Without casts:           -2,   -0.3198557,    -1.839262 (0xC0000000, 0xBEA3C421, 0xBFEB6CEC)
With casts:          1.6705,           -2,    0.2956932 (0x3FD5D2EE, 0xC0000000, 0x3E97651B)
MISMATCH!!!

100000
Without casts:     1.605168,    -1.940973,            2 (0x3FCD7621, 0xBFF871CB, 0x40000000)
With casts:      -0.7341976,     1.686916,            2 (0xBF3BF460, 0x3FD7ECDC, 0x40000000)
MISMATCH!!!

1000000
Without casts:           -2,    0.7777206,     1.835202 (0xC0000000, 0x3F4718B3, 0x3FEAE7E3)
With casts:              -2,    0.8061017,     -1.16992 (0xC0000000, 0x3F4E5CAE, 0xBF95BFF0)
MISMATCH!!!
```

## Test using NUnit Release:

```
10
Without casts:            2,    0.2251637,     1.938193 (0x3FFFFFFF, 0x3E669153, 0x3FF816B6)
With casts:               2,    0.2251346,     1.938198 (0x3FFFFFFE, 0x3E6689B0, 0x3FF816DD)
MISMATCH!!!

100
Without casts:    -1.244922,            2,     0.876503 (0xBF9F599D, 0x3FFFFFFE, 0x3F606280)
With casts:       -1.364468,           -2,     1.616698 (0xBFAEA6E2, 0xC0000000, 0x3FCEEFF4)
MISMATCH!!!

1000
Without casts:    0.0662517,   -0.6017641,            2 (0x3D87AEF9, 0xBF1A0D36, 0x3FFFFFFF)
With casts:       0.6269189,     -1.13679,           -2 (0x3F207DC2, 0xBF918256, 0xBFFFFFFE)
MISMATCH!!!

10000
Without casts:    0.2860791,            2,     1.872738 (0x3E9278F7, 0x40000000, 0x3FEFB5E4)
With casts:        1.832273,            2,   -0.3357688 (0x3FEA87EF, 0x3FFFFFFE, 0xBEABE9E2)
MISMATCH!!!

100000
Without casts:    -1.388948,           -2,   -0.2181758 (0xBFB1C90C, 0xC0000000, 0xBE5F697B)
With casts:         1.48769,           -2,     1.662687 (0x3FBE6CA4, 0xC0000000, 0x3FD4D2F0)
MISMATCH!!!

1000000
Without casts:            2,   -0.2598107,   -0.3831332 (0x40000000, 0xBE8505E9, 0xBEC42A08)
With casts:       -1.767049,            2,    0.8859042 (0xBFE22EAC, 0x40000000, 0x3F62CA9E)
MISMATCH!!!
```

## Console app (32 bit) Debug:

```
10
Without casts:            2,    0.2251443,     1.938195 (0x40000001, 0x3E668C38, 0x3FF816CA)
With casts:               2,    0.2251351,     1.938197 (0x40000000, 0x3E6689D2, 0x3FF816D9)
MISMATCH!!!

100
Without casts:    -1.119223,   -0.2299059,            2 (0xBF8F42AF, 0xBE6B6C76, 0x40000000)
With casts:               2,   -0.5220664,    0.4623849 (0x40000000, 0xBF05A624, 0x3EECBDB6)
MISMATCH!!!

1000
Without casts:    0.4805907,           -2,      1.84443 (0x3EF60FFC, 0xC0000000, 0x3FEC1648)
With casts:              -2,     -1.21301,   -0.4805112 (0xC0000000, 0xBF9B43E6, 0xBEF60590)
MISMATCH!!!

10000
Without casts:           -2,   -0.3198557,    -1.839262 (0xC0000000, 0xBEA3C421, 0xBFEB6CEC)
With casts:          1.6705,           -2,    0.2956932 (0x3FD5D2EE, 0xC0000000, 0x3E97651B)
MISMATCH!!!

100000
Without casts:     1.605168,    -1.940973,            2 (0x3FCD7621, 0xBFF871CB, 0x40000000)
With casts:      -0.7341976,     1.686916,            2 (0xBF3BF460, 0x3FD7ECDC, 0x40000000)
MISMATCH!!!

1000000
Without casts:           -2,    0.7777206,     1.835202 (0xC0000000, 0x3F4718B3, 0x3FEAE7E3)
With casts:              -2,    0.8061017,     -1.16992 (0xC0000000, 0x3F4E5CAE, 0xBF95BFF0)
MISMATCH!!!
```

## Console app (32 bit) Release:

```
10
Without casts:            2,    0.2251637,     1.938193 (0x3FFFFFFF, 0x3E669153, 0x3FF816B6)
With casts:               2,    0.2251346,     1.938198 (0x3FFFFFFE, 0x3E6689B0, 0x3FF816DD)
MISMATCH!!!

100
Without casts:    -1.244922,            2,     0.876503 (0xBF9F599D, 0x3FFFFFFE, 0x3F606280)
With casts:       -1.364468,           -2,     1.616698 (0xBFAEA6E2, 0xC0000000, 0x3FCEEFF4)
MISMATCH!!!

1000
Without casts:    0.0662517,   -0.6017641,            2 (0x3D87AEF9, 0xBF1A0D36, 0x3FFFFFFF)
With casts:       0.6269189,     -1.13679,           -2 (0x3F207DC2, 0xBF918256, 0xBFFFFFFE)
MISMATCH!!!

10000
Without casts:    0.2860791,            2,     1.872738 (0x3E9278F7, 0x40000000, 0x3FEFB5E4)
With casts:        1.832273,            2,   -0.3357688 (0x3FEA87EF, 0x3FFFFFFE, 0xBEABE9E2)
MISMATCH!!!

100000
Without casts:    -1.388948,           -2,   -0.2181758 (0xBFB1C90C, 0xC0000000, 0xBE5F697B)
With casts:         1.48769,           -2,     1.662687 (0x3FBE6CA4, 0xC0000000, 0x3FD4D2F0)
MISMATCH!!!

1000000
Without casts:            2,   -0.2598107,   -0.3831332 (0x40000000, 0xBE8505E9, 0xBEC42A08)
With casts:       -1.767049,            2,    0.8859042 (0xBFE22EAC, 0x40000000, 0x3F62CA9E)
MISMATCH!!!
```

## Console app (any) Debug:

```
10
Without casts:            2,    0.2251443,     1.938195 (0x40000001, 0x3E668C38, 0x3FF816CA)
With casts:               2,    0.2251351,     1.938197 (0x40000000, 0x3E6689D2, 0x3FF816D9)
MISMATCH!!!

100
Without casts:    -1.119223,   -0.2299059,            2 (0xBF8F42AF, 0xBE6B6C76, 0x40000000)
With casts:               2,   -0.5220664,    0.4623849 (0x40000000, 0xBF05A624, 0x3EECBDB6)
MISMATCH!!!

1000
Without casts:    0.4805907,           -2,      1.84443 (0x3EF60FFC, 0xC0000000, 0x3FEC1648)
With casts:              -2,     -1.21301,   -0.4805112 (0xC0000000, 0xBF9B43E6, 0xBEF60590)
MISMATCH!!!

10000
Without casts:           -2,   -0.3198557,    -1.839262 (0xC0000000, 0xBEA3C421, 0xBFEB6CEC)
With casts:          1.6705,           -2,    0.2956932 (0x3FD5D2EE, 0xC0000000, 0x3E97651B)
MISMATCH!!!

100000
Without casts:     1.605168,    -1.940973,            2 (0x3FCD7621, 0xBFF871CB, 0x40000000)
With casts:      -0.7341976,     1.686916,            2 (0xBF3BF460, 0x3FD7ECDC, 0x40000000)
MISMATCH!!!

1000000
Without casts:           -2,    0.7777206,     1.835202 (0xC0000000, 0x3F4718B3, 0x3FEAE7E3)
With casts:              -2,    0.8061017,     -1.16992 (0xC0000000, 0x3F4E5CAE, 0xBF95BFF0)
MISMATCH!!!
```

## Console app (any) Release:

```
10
Without casts:            2,    0.2251397,     1.938196 (0x40000000, 0x3E668B04, 0x3FF816D0)
With casts:               2,    0.2251397,     1.938196 (0x40000000, 0x3E668B04, 0x3FF816D0)

100
Without casts:     1.439869,            2,    0.1769729 (0x3FB84DA0, 0x40000000, 0x3E353864)
With casts:        1.439869,            2,    0.1769729 (0x3FB84DA0, 0x40000000, 0x3E353864)

1000
Without casts:   -0.4723696,            2,    -1.706456 (0xBEF1DA6C, 0x40000000, 0xBFDA6D2A)
With casts:      -0.4723696,            2,    -1.706456 (0xBEF1DA6C, 0x40000000, 0xBFDA6D2A)

10000
Without casts:    -1.075579,    -1.523837,            2 (0xBF89AC96, 0xBFC30D17, 0x40000000)
With casts:       -1.075579,    -1.523837,            2 (0xBF89AC96, 0xBFC30D17, 0x40000000)

100000
Without casts:    -1.200156,    0.1734155,           -2 (0xBF999EB7, 0x3E3193D8, 0xC0000000)
With casts:       -1.200156,    0.1734155,           -2 (0xBF999EB7, 0x3E3193D8, 0xC0000000)

1000000
Without casts:     1.621604,    0.3454828,            2 (0x3FCF90BC, 0x3EB0E320, 0x40000000)
With casts:        1.621604,    0.3454828,            2 (0x3FCF90BC, 0x3EB0E320, 0x40000000)
```