import unittest
from os import path
import pandas as pd

from pandas.util.testing import assert_frame_equal

from src.prepare.main import read_data, transpose_data

test_dir = path.dirname(__file__)
orig_path = 'data/fake-data-orig.csv'
out_path = 'data/fake-data-out.csv'


def copy_types_from_another(out_df: pd.DataFrame, transposed_df: pd.DataFrame) -> pd.DataFrame:
    for column in transposed_df.columns:
        out_df[column] = out_df[column].astype(transposed_df[column].dtype.name)

    return out_df


class TestStringMethods(unittest.TestCase):

    def test_transpose_equalto_out_without_index(self):
        orig_df = read_data(orig_path).reset_index(drop=True)
        out_df = read_data(out_path).reset_index(drop=True)

        transposed_df = transpose_data(orig_df).reset_index(drop=True)

        transposed_df = copy_types_from_another(transposed_df, out_df)

        assert_frame_equal(transposed_df, out_df, check_dtype=False)


if __name__ == '__main__':
    unittest.main()
