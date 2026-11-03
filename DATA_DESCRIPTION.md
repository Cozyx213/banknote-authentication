# Data Description

## Dataset
**Banknote Authentication** dataset, UCI Machine Learning Repository
https://archive.ics.uci.edu/dataset/267/banknote+authentication

- **Donated:** 2012, by Volker Lohweg and Helene Dörksen, Ostwestfalen-Lippe University of Applied Sciences, Germany
- **File in this project:** `neural/Data/data_banknote_authentication.csv`
- **Task:** binary classification. Is a banknote **genuine (0)** or **forged (1)**?

## How the data was made
Genuine and forged banknote-like specimens were photographed with an industrial camera normally used for print inspection.
The images are 400 × 400 pixels, grayscale, at a resolution of about 660 dpi.
A **wavelet transform** was applied to each image, and 4 numbers describing the texture of the result were extracted.
These 4 numbers are the input features. The network never sees the images themselves.

## Columns
| Column | Type | Meaning |
|---|---|---|
| `variance` | number | Variance of the wavelet-transformed image: how spread out the values are |
| `skewness` | number | Skewness of the wavelet-transformed image: how lopsided the value distribution is |
| `curtosis` | number | Kurtosis of the wavelet-transformed image: how peaked or heavy-tailed the distribution is |
| `entropy` | number | Entropy of the image: how much randomness or detail it contains |
| `class` | 0 or 1 | Label: 0 = genuine, 1 = forged |

The UCI page does not state which class number means "genuine". 0 = genuine / 1 = forged is the common convention and is used in this project.

## Size and balance
| | Rows | Share |
|---|---|---|
| Genuine (0) | 762 | 56% |
| Forged (1) | 610 | 44% |
| **Total** | **1,372** | |

- **No missing values.** Every row has all 5 columns filled with numbers.
- **The classes are close to balanced**, so accuracy is a meaningful metric. Precision and recall are still reported.
- **The file is sorted by class** (all genuine rows first, then all forged rows), so it must be shuffled before splitting.
- **24 rows are exact duplicates** of another row. They were kept, as in the original dataset.

## Feature statistics
| Feature | Min | Max | Mean | Std. dev. | Mean (genuine) | Mean (forged) |
|---|---|---|---|---|---|---|
| variance | −7.04 | 6.82 | 0.43 | 2.84 | 2.28 | −1.87 |
| skewness | −13.77 | 12.95 | 1.92 | 5.87 | 4.26 | −0.99 |
| curtosis | −5.29 | 17.93 | 1.40 | 4.31 | 0.80 | 2.15 |
| entropy | −8.55 | 2.45 | −1.19 | 2.10 | −1.15 | −1.25 |

What this shows:
- **Variance and skewness separate the classes best.** Genuine notes have clearly positive averages,
  forged notes negative ones. This is why even a small network can classify the data almost perfectly.
- **Entropy barely differs between the classes** (−1.15 vs −1.25), so on its own it carries little information.
  It can still help in combination with the other features.
- **The features have very different ranges** (curtosis spans about 23 units, entropy about 11).
  This is why they are MinMax-scaled to [0, 1] before training (see below).

## How the data is used in this project
1. **Parsing:** the CSV is read with invariant culture (`.` as the decimal point). The header and any line that does not have
   exactly 5 numeric columns are skipped.
2. **Split:** the rows are shuffled with a fixed seed (42) and split **80% train (1,098 rows: 606 genuine, 492 forged)
   / 20% test (274 rows: 156 genuine, 118 forged)**. The fixed seed gives the same split on every run.
3. **Scaling:** each feature is MinMax-scaled to [0, 1] with `x' = (x − min) / (max − min)`. The min and max are taken from the
   **training set only**, so nothing about the test set leaks into training. Test values can therefore fall slightly outside [0, 1].

## Limitations
- The specimens are "banknote-like" and were photographed under controlled industrial conditions.
  Real-world notes (worn, folded, photographed with a phone) may look different.
- The exact duplicates mean that a few test rows may also appear in the training set, which can make the test score
  very slightly optimistic.
- The dataset is small (1,372 rows) and nearly linearly separable, so near-perfect scores are expected
  and the data does not show how the network would cope with a harder problem.
