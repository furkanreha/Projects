library(ISLR)
library(tidyverse)
library(funModeling)
library(caret)
library(pROC)
library(class)
library(e1071)
library(kernlab) 
library(ROCR) 
library(neuralnet)
library(GGally)
library(nnet)
library(rpart)
library(cli)
library(tree)
library(rpart.plot)
library(randomForest)
library(gbm)
library(xgboost)
library(DiagrammeR)
library(mlbench)

df<-read.csv(file.choose(),header = T,sep = " ")
str(df)
df2<-df[,1:3]

train_yy<- df$period

train_indeks<-createDataPartition(df2$day,p =0.8,list=F,times=1)

train <- df2[train_indeks,]
test <- df2[-train_indeks,]

train_x <- train %>% dplyr::select(-day)
train_y <- train$day

test_x <- test %>% dplyr::select(-day)
test_y <- test$day

training <- data.frame(train_x, day = train_y)

knn_train <- train
knn_test <- test

knn_train$action<-as.factor(knn_train$action)
knn_test$action<-as.factor(knn_test$action)

knn_train <- knn_train %>% dplyr::select(-day)
knn_test <- knn_test %>% dplyr::select(-day)

knn_fit<-knn(cbind(knn_train$action,knn_train$id),cbind(knn_test$action,knn_test$id),cl=train_y,k=19)
table(knn_fit,test_y)
summary(test_y)

ctrl <- trainControl(method="repeatedcv",repeats = 3)

knnFit <- train(day ~ ., data = train, method = "knn", trControl = ctrl, preProcess = c("center","scale"), tuneLength = 20)
plot(knnFit)

knnFit
summary(knnFit)

real_test<-read.csv(file.choose(),header = T,sep = " ")
tahmin<-knn(cbind(knn_train$action,knn_train$id),cbind(real_test$action,real_test$id),cl=train_y,k=21)
str(real_test)
real_test$day<-tahmin

train2<-df[train_indeks,]
test2<-df[-train_indeks,]

train2_x<-train2 %>% dplyr::select(-period)
train2_y<-train2$period          

test2_x<-test2 %>% dplyr::select(-period)
test_y<-test2$period

training<-data.frame(train2_x,period=train2_y)

knn_fit2<-knn(cbind(train2_x$action,train2_x$day,train2_x$ï..id),cbind(test2_x$action,test2_x$day,test2_x$ï..id),cl=train2_y,k=21)

knn_fit2
table(knn_fit2)

tahmin2<-knn(cbind(train2_x$action,train2_x$day,train2_x$ï..id),cbind(real_test$action,real_test$ï..id,real_test$day),cl=train2_y,k=19)

real_test$period<-tahmin2

write.table(real_test, file = "result.txt",row.names = FALSE, col.names = FALSE)

