# MG_Infinity Game Development

## 背景
- cl0wnが興味本位で持ち掛けたUnityでの音ゲー開発に今のメンバーが加わった。
- 設計、開発、公開のプロセスを体験することを目的としている。
- 2019/4/1までに公開できない場合、どこかしらに一人1000円の寄付をします。

## 仕様
ゲーム画面は以下の6つのシーンからなる。
- start([ko7ka](https://github.com/ko7ka))
- select([mikaner](https://github.com/Mikaner))
- option([mikaner](https://github.com/Mikaner))
- play([cl0wn](https://github.com/cl0wn))
- pause([cl0wn](https://github.com/cl0wn), [gbtss](https://github.com/syun1015))
- result([gbtss](https://github.com/syun1015))

## 開発フロー(collaboratorに限る)
1. (初回のみ)`git clone -b <自分のブランチ名> https://github.com/hakomori64/MG_Infinity.git`
2. (初回のみ)`cd MG_Infinity; git remote add upstream https://github.com/hakomori64/MG_Infinity.git`
3. `git pull upstream master`
4. 作業を行う
5. `git add .`
6. `git commit -m "コミットメッセージ"`
7. `git push upstream <自分のブランチ名>`
8. githubから***pull request master \<\- \<自分のブランチ名\>***を行う
