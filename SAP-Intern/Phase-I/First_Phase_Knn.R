library(class)

data<-read.csv(file.choose(),header = T,sep = " ")

train1<-data[,c(1)]
train2<-data[,c(2)]
train3<-data[,c(3)]

test<-read.csv(file.choose(),header = T,sep = " ")
test1<-test[,c(1)]
test2<-test[,c(2)]

foracast<-knn(cbind(train1, train2), cbind(test1, test2), data$day)
test$day_tahmin<-foracast

test3<-test[,c(3)]
foracast2<-knn(cbind(train1, train2, train3), cbind(test1, test2, test3), data$period)
test$period_tahmin<-foracast2
foracast2

write.table(test, file = "result.txt",row.names = FALSE, col.names = FALSE)

