import pandas as pd

from src.cpu.cpu_rank_scraper import get_processors


def __build_processor_up_replacement_dict(unique_processor_names: pd.Series,
                                          all_processors_sorted: pd.DataFrame) -> dict:
    result = {}
    for p in unique_processor_names:
        rank = all_processors_sorted[all_processors_sorted.processor_name.str.contains(p)].iloc[0]['rank']
        up_processor_name = all_processors_sorted[all_processors_sorted['rank'] < rank].iloc[-1]['processor_name']
        result[p] = up_processor_name
    return result


def __build_processor_down_replacement_dict(unique_processor_names: pd.Series,
                                            all_processors_sorted: pd.DataFrame) -> dict:
    result = {}
    for p in unique_processor_names:
        rank = all_processors_sorted[all_processors_sorted.processor_name.str.contains(p)].iloc[0]['rank']
        up_processor_name = all_processors_sorted[all_processors_sorted['rank'] > rank].iloc[0]['processor_name']
        result[p] = up_processor_name
    return result


def expand_df_with_similar_processors_from_cpubenchmark(df: pd.DataFrame) -> pd.DataFrame:
    all_processors_sorted = get_processors().sort_values('rank')
    unique_processor_names = pd.unique(df['processor_name'].dropna())
    up_replacements = __build_processor_up_replacement_dict(unique_processor_names, all_processors_sorted)
    down_replacements = __build_processor_down_replacement_dict(unique_processor_names, all_processors_sorted)
    up_processors = df.replace({'processor_name': up_replacements})
    down_processors = df.replace({'processor_name': down_replacements})
    return df.append(up_processors, ignore_index=True).append(down_processors, ignore_index=True)


def expand_df_with_ssd_for_gamers_programmers(df: pd.DataFrame) -> pd.DataFrame:
    hdd_users = df[(df.disk_type == 'hdd') & ((df.mainuse == 'programming') | (df.mainuse == 'gaming'))]
    hdd_users.loc[hdd_users['disk_type'] == 'hdd', 'disk_type'] = 'ssd'
    return df.append(hdd_users, ignore_index=True)