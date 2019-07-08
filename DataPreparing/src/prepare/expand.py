import pandas as pd

from src.decorators.pandas_config import disable_chained_assignment

__cpu_rank_url = 'https://www.cpubenchmark.net/cpu_list.php'


def __get_processors() -> pd.DataFrame:
    processors = pd.read_html(__cpu_rank_url)[4]
    processors.columns = ['processor_name', 'passmark', 'rank', 'value', 'price']
    return processors


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
    all_processors_sorted = __get_processors().sort_values('rank')
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


def __add_price_by_fieldinterest_mainuse(row: pd.Series):
    fieldinterest_update_dict = {
        'computers': 1000,
        'home': -500,
        'office': 500
    }
    mainuse_update_dict = {
        'programming': 1000,
        'personal use': 300,
        'gaming': 1000,
        'work': 500
    }
    row['price'] = row['price'] + \
                   fieldinterest_update_dict[row['fieldinterest']] + \
                   mainuse_update_dict[row['mainuse']]
    return row


def expand_prices_by_fieldinterest_mainuse(df: pd.DataFrame) -> pd.DataFrame:
    added = df.apply(__add_price_by_fieldinterest_mainuse, axis=1)
    return df.append(added)


def expand_ddrsocket_by_computertype(df: pd.DataFrame) -> pd.DataFrame:
    desktops = df.loc[df['computertype'] == 'desk']
    desktops.loc[:, 'motherboard_ddrsockets'] = 4

    ddr3 = df.loc[(df['memory_type_number'] == '3') & (df['computertype'] == 'laptop')]
    ddr3.loc[:, 'motherboard_ddrsockets'] = 2

    return df.append(desktops).append(ddr3)
