import pandas as pd

from src.cpu.cpu_rank_scraper import get_processors
from src.decorators.pandas_config import disable_chained_assignment


def __build_processor_replacement_dict(unique_processor_names: pd.Series,
                                          all_processors_sorted: pd.DataFrame, up: bool) -> dict:
    result = {}
    for p in unique_processor_names:
        rank = all_processors_sorted[all_processors_sorted.processor_name.str.contains(p)].iloc[0]['rank']
        if up:
            new_processor_name = all_processors_sorted[all_processors_sorted['rank'] < rank].iloc[-1]['processor_name']
        else:
            new_processor_name = all_processors_sorted[all_processors_sorted['rank'] > rank].iloc[0]['processor_name']

        if new_processor_name in result:
            new_processor_name = new_processor_name + " "

        result[p] = new_processor_name
    return result
    pass


def expand_df_with_similar_processors_from_cpubenchmark(df: pd.DataFrame) -> pd.DataFrame:
    all_processors_sorted = get_processors().sort_values('rank')
    unique_processor_names = pd.unique(df['processor_name'].dropna())
    up_replacements = __build_processor_replacement_dict(unique_processor_names, all_processors_sorted, True)
    down_replacements = __build_processor_replacement_dict(unique_processor_names, all_processors_sorted, False)
    up_processors = df.replace({'processor_name': up_replacements})
    down_processors = df.replace({'processor_name': down_replacements})
    return df.append(up_processors, ignore_index=True).append(down_processors, ignore_index=True)


@disable_chained_assignment
def expand_df_with_ssd_for_gamers_programmers(df: pd.DataFrame) -> pd.DataFrame:
    hdd_users = df[(df.disk_type == 'hdd') & ((df.mainuse == 'programming') | (df.mainuse == 'gaming'))]
    hdd_users['disk_type'] = 'ssd'
    return df.append(hdd_users, ignore_index=True)
