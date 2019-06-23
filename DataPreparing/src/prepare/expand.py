import pandas as pd

from src.cpu.cpu_rank_scraper import get_processors


def __build_processor_up_replacement_dict(unique_processor_names: pd.Series,
                                          all_processors_sorted: pd.DataFrame) -> dict:
    result = {}
    for p in unique_processor_names:
        try:
            rank = all_processors_sorted[all_processors_sorted.processor_name.str.contains(p)].iloc[0]['rank']
            up_processor_name = all_processors_sorted[all_processors_sorted['rank'] < rank].iloc[-1]['processor_name']
            result[p] = up_processor_name
        except Exception as ee:
            print(f"rank: {rank}")
            print(f"data:{all_processors_sorted[all_processors_sorted['rank'] < rank]}")
            return result
    return result


def expand_df_with_similar_processors_from_cpubenchmark(df: pd.DataFrame) -> pd.DataFrame:
    all_processors_sorted = get_processors().sort_values('rank')
    unique_processor_names = pd.unique(df['processor_name'].dropna())
    up_replacements = __build_processor_up_replacement_dict(unique_processor_names, all_processors_sorted)
    df = df.append(df.replace({'processor_name': up_replacements}), ignore_index=True)
    return df
