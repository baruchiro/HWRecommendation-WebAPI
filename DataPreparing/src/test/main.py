import unittest
from os import path
from typing import List, Tuple

import pandas as pd
from pandas.util.testing import assert_frame_equal, assert_series_equal

from src.prepare.main import read_data, transpose_data
from src.research.utils import get_correlation_matrix, correlation_matrix_to_sorted_pairs

test_dir = path.dirname(__file__)
orig_path = 'data/fake-data-orig.csv'
out_path = 'data/fake-data-out.csv'
out_dtypes_path = 'data/fake-data-out.dtypes.csv'


def copy_types_from_another(out_df: pd.DataFrame, transposed_df: pd.DataFrame) -> pd.DataFrame:
    for column in transposed_df.columns:
        out_df[column] = out_df[column].astype(transposed_df[column].dtype.name)

    return out_df


class TestStringMethods(unittest.TestCase):

    def test_transpose_equalto_out_without_index(self):
        orig_df = read_data(orig_path).reset_index(drop=True)
        out_df = read_data(out_path).reset_index(drop=True)
        out_dtypes = read_data(out_dtypes_path, header=None).set_index(0)
        out_dtypes = pd.Series(out_dtypes[1], out_dtypes.index)

        transposed_df = transpose_data(orig_df).reset_index(drop=True)

        transposed_df = copy_types_from_another(transposed_df, out_df)

        assert_frame_equal(transposed_df, out_df, check_dtype=False)
        assert_series_equal(transposed_df.dtypes, out_dtypes, check_names=False)

    def test_track_correlation(self):
        def assert_correlation_fields_count(correlation: int, rows: int, fields: List[Tuple[str, str]]):
            actual_rows = corr.loc[corr[0] == correlation]
            self.assertEqual(actual_rows[0].count(), rows,
                             f'expected {rows} rows with {correlation} correlation but found '
                             f'{actual_rows[0].count()} rows.')

            for tuple_fields in fields:
                lines = len(actual_rows.loc[
                                actual_rows['both'].str.contains(tuple_fields[0]) &
                                actual_rows['both'].str.contains(tuple_fields[1])
                                ])
                self.assertEqual(lines, 1, f'expected exacly one rows with {tuple_fields} but found {lines} rows')

        orig_df = read_data(orig_path).reset_index(drop=True)

        transposed_df = transpose_data(orig_df).reset_index(drop=True)

        corr = correlation_matrix_to_sorted_pairs(get_correlation_matrix(transposed_df))
        corr[0] = (corr[0] * 10).astype(int)
        corr['both'] = corr['level_0'] + corr['level_1']

        assert_correlation_fields_count(correlation=9, rows=0, fields=[])
        assert_correlation_fields_count(correlation=8, rows=2, fields=[
            ('motherboard_ddrsockets', 'computertype'),
            ('disk_model', 'computertype')
        ])
        assert_correlation_fields_count(correlation=7, rows=1, fields=[
                ('computertype', 'processor_mhz'),
        ])
        assert_correlation_fields_count(correlation=0, rows=138, fields=[])


if __name__ == '__main__':
    unittest.main()
