# Banknote Authentication with a Custom MLP in C#

Binary classification (genuine vs. forged banknote) using our own 3-layer neural network
(`NeuralNet`, `INeuron`, `HNeuron`, `ONeuron`: sigmoid activations, backpropagation).
No machine-learning libraries are used. The only extra .NET library is the built-in
`System.Windows.Forms.DataVisualization` Chart control, used purely to draw the graphs.

## How to run
Requirements: Windows, Visual Studio with the **.NET desktop development** workload (.NET Framework 4.8, WinForms).

1. Open `neural.slnx` in Visual Studio and press **F5**.
2. Click **Load Dataset** and choose `neural/Data/data_banknote_authentication.csv`.
3. Set **hidden neurons / learning rate / epochs** in the boxes at the top left (default `8 / 1.0 / 500`).
4. **Create NN**: loads the CSV, splits it 80/20, MinMax-scales it and creates a new 4 → hidden → 1 network.
5. **Train NN**: trains for the given number of epochs. It then:
   - prints the train and test loss every 100 epochs,
   - prints the confusion matrix, Accuracy, Precision, Recall, F1 and MSE for both the train and test sets,
   - saves `training_log.csv` (train and test MSE for every epoch) next to `neural.exe`,
   - fills in the graphs (see [Visualizations](#visualizations)).

   Pressing it again continues training the same network.
6. **Run NN**: type the 4 feature values of a banknote and get the network's output (≥ 0.5 = forged).
7. **Tune Hyperparameters** (optional): trains one fresh network per combination in a grid, prints a table
   and saves `tuning_results.csv`. It does not change the network made with Create NN.

## Files
| File | Purpose |
|---|---|
| `BackNeural 1.cs`, `InputN 1.cs`, `HiddenN 1.cs`, `OuputN 1.cs` | Neural network engine (from the previous activity) |
| `Banknote.cs` | CSV loading, train/test split, MinMax scaling, training loop, metrics (confusion matrix, accuracy, precision, recall, F1, MSE, ROC/AUC) |
| `Form1.cs` | Buttons: Load Dataset, Create / Train / Run NN, Tune Hyperparameters |
| `Form1.Charts.cs` | The evaluation graphs (loss curve, metrics, confusion matrix, ROC curve) |
| `Data/data_banknote_authentication.csv` | Dataset |

Only change to the engine: the learning rates were `const`. A constructor
`NeuralNet(input, hidden, output, lrOutput, lrInput)` was added so they can be tuned.

## Data description
**Banknote Authentication** dataset from the UCI Machine Learning Repository
(https://archive.ics.uci.edu/dataset/267/banknote+authentication), donated in 2012 by
Volker Lohweg and Helene Dörksen (Ostwestfalen-Lippe University of Applied Sciences, Germany).

Genuine and forged banknote-like specimens were photographed with an industrial print-inspection camera
(400×400 px, grayscale, about 660 dpi). A wavelet transform was applied to each image and 4 texture features were extracted:

| Feature | Meaning |
|---|---|
| variance | variance of the wavelet-transformed image |
| skewness | skewness of the wavelet-transformed image |
| curtosis | kurtosis of the wavelet-transformed image |
| entropy | entropy of the image |
| class (label) | 0 = genuine, 1 = forged |

1,372 rows (762 genuine = 56%, 610 forged = 44%), no missing values. The classes are close to balanced,
so accuracy is a meaningful metric. The CSV is sorted by class (all 0s first), which is why it must be shuffled before splitting.
The UCI page does not say which class number means "genuine". 0 = genuine is the common convention and is used here.

## Preprocessing
- **Parsing:** the CSV is read with `CultureInfo.InvariantCulture` (`.` as decimal point on any PC).
  Rows that are not fully numeric, such as the header, are skipped.
- **Split:** the rows are shuffled with a fixed seed (42), so the split is the same every run, then split
  **80% train (1,098 rows) / 20% test (274 rows)**.
- **MinMax scaling** to [0, 1]: `x' = (x − min) / (max − min)`. The min and max come from the **training set only**,
  so no information from the test set leaks into training. The same min/max are reused to scale the values typed into Run NN.

## Training log (8 hidden, lr 1.0, 500 epochs)
```
Epoch     1   train MSE = 0.220983   test MSE = 0.099162
Epoch   100   train MSE = 0.001326   test MSE = 0.000330
Epoch   200   train MSE = 0.000139   test MSE = 0.000101
Epoch   300   train MSE = 0.000075   test MSE = 0.000066
Epoch   400   train MSE = 0.000045   test MSE = 0.000060
Epoch   500   train MSE = 0.000037   test MSE = 0.000035
```
The full per-epoch log is saved to `training_log.csv`.
- **Train MSE** is averaged over each epoch while the weights are updating.
- **Test MSE** is measured after each epoch with the weights frozen.

Initial weights are random, so the exact numbers change slightly each run.
In 5 repeated runs of this configuration, test accuracy was 100% every time and test MSE was between 0.00003 and 0.00004.

## Hyperparameter tuning
Grid: hidden neurons {2, 4, 8, 16} × learning rate {0.01, 0.1, 0.5, 1.0} × epochs {100, 250, 500} = 48 runs.
Each run trains a new network on the training set and is scored on the test set.

**Test accuracy:**

| hidden \ lr, epochs | 0.01, 100 | 0.01, 500 | 0.1, 100 | 0.1, 500 | 0.5, 100 | 0.5, 500 | 1.0, 100 | 1.0, 500 |
|---|---|---|---|---|---|---|---|---|
| 2  | 89.78% | 98.54% | 98.54% | 98.91% | 98.91% | 100% | 100% | 100% |
| 4  | 94.89% | 98.54% | 98.91% | 100%   | 100%   | 100% | 100% | 100% |
| 8  | 94.16% | 98.54% | 98.54% | 100%   | 100%   | 100% | 100% | 100% |
| 16 | 94.89% | 98.54% | 98.91% | 100%   | 100%   | 100% | 100% | 100% |

**Test MSE at 500 epochs** (lower is better; this separates the configurations that all reach 100%):

| hidden \ lr | 0.01 | 0.1 | 0.5 | 1.0 |
|---|---|---|---|---|
| 2  | 0.01267 | 0.00932 | 0.00019 | 0.00017 |
| 4  | 0.01226 | 0.00152 | 0.00017 | 0.00002 |
| 8  | 0.01202 | 0.00129 | 0.00007 | 0.00003 |
| 16 | 0.01191 | 0.00122 | 0.00011 | 0.00003 |

The **Tune Hyperparameters** button runs the grid defined at the top of `btnTuneBanknote_Click` in `Form1.cs`.
To reproduce the tables above, set those arrays to the values listed in the grid line above.

**Findings:**
- **Learning rate mattered most.** At 0.01 the network learns too slowly: only 89.8–94.9% after 100 epochs,
  and it plateaus around 98.5% at 500 epochs. At 0.5–1.0 it reaches 100% within 100–250 epochs.
- **Epochs:** more epochs keep lowering the MSE (the network becomes more confident).
  At lr ≥ 0.5, accuracy is already at 100%, so the extra epochs mainly improve MSE.
- **Hidden neurons:** 2 neurons is the weakest (e.g. 98.9% at lr 0.1 / 500 where larger networks reach 100%).
  4–16 behave almost the same, so a bigger network brings no benefit on this data.
- **Chosen configuration: 8 hidden neurons, learning rate 1.0, 500 epochs.** It reaches 100% test accuracy,
  has one of the lowest test MSEs, and was stable across 5 repeated runs. 4 hidden neurons does equally well
  and would be the choice if a smaller network were preferred.

## Evaluation
Positive class = **forged** (1); threshold 0.5.

| Metric | Meaning here |
|---|---|
| Accuracy | share of all notes classified correctly |
| Precision | of the notes flagged as forged, how many really were forged |
| Recall | of the forged notes, how many were caught |
| F1 | harmonic mean of precision and recall |
| MSE | average squared difference between the output (0–1) and the label; shows how *confident* the predictions are |
| ROC AUC | how well the output separates the two classes across every possible threshold (1.0 = perfect, 0.5 = random guessing) |

**Chosen model (8 / 1.0 / 500):**
```
TRAIN (1,098 notes)                    TEST (274 notes, never seen in training)
              genuine  forged                        genuine  forged
  genuine        606       0             genuine        156       0
  forged           0     492             forged           0     118
Accuracy 100%  Precision 100%            Accuracy 100%  Precision 100%
Recall   100%  F1        100%            Recall   100%  F1        100%
MSE 0.00004                              MSE 0.00004    ROC AUC 1.0000
```

**Analysis:**
- **Recall is the most important metric for this problem.** A missed forgery (false negative) is the costly mistake,
  because a fake note gets accepted. The model missed none.
- **For comparison, an under-trained model** (lr 0.01, 100 epochs, 2 hidden) still gets 89.8% accuracy and 95.9% precision,
  but only **79.7% recall**: it lets about 1 in 5 forgeries through. This is why accuracy alone is not enough.
- **No overfitting:** train and test results are both 100%, and the test loss curve follows the train loss curve down
  without rising again.
- **Why a small network is enough:** the dataset is nearly linearly separable, so even 2–4 hidden neurons can
  classify it perfectly once the learning rate is high enough.
- **Caveat:** the configuration was chosen using the test set, which makes the test score slightly optimistic.
  A stricter setup would tune on a separate validation split and use the test set only once at the end.

## Visualizations
After **Train NN**, the tabs under the Create/Train panel show:

| Loss curve | Metrics |
|---|---|
| ![Loss curve](docs/loss.png) | ![Metrics](docs/metrics.png) |
| Train and test MSE per epoch (log scale). Both fall together, so no overfitting. | Accuracy / Precision / Recall / F1 for train vs test. |

| Confusion matrix | ROC curve |
|---|---|
| ![Confusion matrix](docs/confusion.png) | ![ROC curve](docs/roc.png) |
| Green = correct, red = mistakes, darker = more notes. | Test set. The curve hugs the top-left corner (AUC 1.0); the dashed line is random guessing. |
