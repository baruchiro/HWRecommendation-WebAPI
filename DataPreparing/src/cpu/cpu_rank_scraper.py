from bs4 import BeautifulSoup
import requests
import pandas as pd

__cpu_rank_url = 'https://www.cpubenchmark.net/cpu_list.php'


def __get_html_request() -> str:
    response = requests.get(__cpu_rank_url)
    if response.status_code != 200:
        raise ConnectionError(response.text)
    return response.text


def __scrap_processors_from_html(html) -> list:
    soup = BeautifulSoup(html, 'html.parser')
    tbody = soup.find(id='cputable').find('tbody')
    trs = tbody.findAll('tr')
    for tr in trs:
        tds = tr.findAll('td')
        yield [td.findAll(text=True)[0] for td in tds[0:3]]


def __parse_processors_to_structure(processors: list) -> pd.DataFrame:
    df = pd.DataFrame(processors)
    df.columns = ['processor_name', 'passmark', 'rank']
    df['processor_name'] = df['processor_name'].astype(str)
    df['passmark'] = df['passmark'].astype(int)
    df['rank'] = df['rank'].astype(int)
    return df


def __pull_cpu_list_to_parsed_list():
    html = __get_html_request()
    processors = __scrap_processors_from_html(html)
    parsed_processors = __parse_processors_to_structure(processors)
    return parsed_processors


def get_processors() -> pd.DataFrame:
    return __pull_cpu_list_to_parsed_list()
