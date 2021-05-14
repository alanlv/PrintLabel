# PrintLabel

### 批量列印小工具

#### 需要工具
    1.codesoft软件,设置软件端口连接或者TCP/IP 连接
    2.用codesoft设计label档
#### 功能
    导入csv文件, 调用codesoft7 label图档实现批量打印label
#### other目录
    测试案例与文档
#### Add ZPL
    ZPL 指令打印可以省去调用模板，连接打印机后通过端口可以直接打印
    ^XA
    ^FO50,60^A0,40^FDWorld's Best Griddle^FS
    ^FO60,120^BY3^BCN,60,,,,A^FD1234ABC^FS
    ^FO25,25^GB380,200,2^FS
    ^XZ

![img](https://fyg1998.github.io/Picture/zpl.png)
