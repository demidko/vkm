import re
_pattern = re.compile(r'(/[a-zA-Z\d]{6,})/.*?[a-zA-Z\d]+?(/index.m3u8)')
def to_mp3(url):
    q = _pattern.findall(url)
    if(len(q)!=1 or len(q[0])!=2):return url;
    return url.replace(q[0][0],'').replace(q[0][1],'.mp3')