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

df2<-df[,1:3]
train_indeks <- createDataPartition(df2$freq, p = .7, list = FALSE, times = 1)

train <- df2[train_indeks,]
test  <- df2[-train_indeks,]

train_x<-train %>% dplyr::select(-freq)
train_y<-train$freq

test_x<-test %>% dplyr::select(-freq)
test_y<-test$freq

nnet_fit <- nnet(freq ~., train, size = 3, decay = 0.1)

summary(nnet_fit)

real_test<-read.csv(file.choose(),header = T,sep = " ")
pred<-predict(nnet_fit,test_x,type="class")

table(pred,test_y)
summary(test_y)

levels(train$freq)<-make.names(levels(factor(train$freq)))
train_y<-train$freq

ctrl <- trainControl(method = "cv", number = 10)

ysa_grid <- expand.grid(
  decay = c(0.001,0.01, 0.1),
  size =  (1:10))

tahmin<-predict(nnet_fit,real_test,type="class")
real_test$freq<-tahmin

train2_indeks<- createDataPartition(df$period,p = 0.8,list=F,times = 1)


train2<-df[train2_indeks,]
test2<- df[-train2_indeks,]

test2_x<-test2 %>% dplyr::select(-period)
test2_y<-test2$period

nnet_period<-nnet(period ~.,train2, size=3,decay=0.01)

pred_2<-predict(nnet_period,test2_x,type="class")

table(test2_y,pred_2)
summary(test2_y)

tahmin_period<- predict(nnet_period,real_test,type="class")

real_test$period<-tahmin_period
write.table(real_test, file = "result.txt",row.names = FALSE, col.names = FALSE)
