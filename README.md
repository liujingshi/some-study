# 一些研究
这个库记录一些技术的研究的 Demo 代码（之所以要建立这个库，一是向保存代码，二是做代码同步，毕竟不同电脑之间云同步代码不好）

# 消息队列

消息队列的研究

这里选型 RabbitMQ

RabbitMQ 基本使用

---
下面的不算 是错误的

运行环境 Kubernetes，Sidecar 模式，使用 Dapr 作为 Sidecar

![MQ-process](./md-images/MQ-process.png)

项目需要的HOSTS

```hosts
47.74.22.216	mysql.coder.com
47.74.22.216	permission.coder.com
47.74.22.216	integral.coder.com
47.74.22.216	message.coder.com
47.74.22.216	payment.coder.com
47.74.22.216	order.coder.com
```

