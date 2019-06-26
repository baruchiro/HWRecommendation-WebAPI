import pandas as pd
import numpy as np


def get_correlation_matrix(df: pd.DataFrame) -> pd.DataFrame:
    types = dict(df.dtypes)
    string_columns = [c for c in types if not np.issubdtype(types[c], np.number)]
    for column in string_columns:
        df[column] = pd.factorize(df[column])[0]

    return df.corr()
