# Data Description

**Banknote Authentication** dataset from the UCI Machine Learning Repository
(https://archive.ics.uci.edu/dataset/267/banknote+authentication), donated in 2012 by Volker Lohweg and Helene Dörksen.
File: `neural/Data/data_banknote_authentication.csv`.

**Task:** classify a banknote as **genuine (0)** or **forged (1)**.

**How it was made:** genuine and forged banknote-like specimens were photographed with an industrial print-inspection camera
(400 × 400 px, grayscale). A wavelet transform was applied to each image, and 4 texture features were extracted.

| Column | Meaning |
|---|---|
| `variance` | variance of the wavelet-transformed image |
| `skewness` | skewness of the wavelet-transformed image |
| `curtosis` | kurtosis of the wavelet-transformed image |
| `entropy` | entropy of the image |
| `class` | 0 = genuine, 1 = forged |

**Size:** 1,372 rows: 762 genuine (56%) and 610 forged (44%). No missing values.

**Notes:**
- Variance and skewness separate the classes best (average variance: +2.28 genuine vs −1.87 forged).
  Entropy barely differs between them.
- The features have very different ranges (e.g. curtosis −5.3 to 17.9, entropy −8.5 to 2.4), so they are MinMax-scaled to [0, 1].
- The file is sorted by class, so it is shuffled (seed 42) before the 80/20 train/test split (1,098 / 274 rows).
- 24 rows are exact duplicates, and the dataset is small and nearly linearly separable, so near-perfect test scores are expected.
