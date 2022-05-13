from abc import *
from enum import Enum
from class_file import DictUseAbleContainer
from multiprocessing import Process, Queue, SimpleQueue, freeze_support
import pickle

class MessageType(Enum):
    Empty_Message = 0
    Request_ImageAnalysis_ImagePath = 1
    Request_ImageSave_JsonPath =2
    Request_Removebg_ImagePath = 3
    Request_FindDetectedObjects = 4
    Request_FindSubCategory_Top = 5
    Request_FindSubCategory_Bottom = 6
    Request_FindSubCategory_Outer = 7
    Request_FindSubCategory_Overall = 8
    Request_FindPattern_Top = 9
    Request_FindPattern_Bottom =10
    Request_FindPattern_Outer = 11
    Request_FindPattern_Overall = 12
    Request_FindStyle = 13
    Receive_ImagePath_RemoveBG = 14
    Receive_ImagePath_Original = 15
    Receive_JsonPath_ImageList = 16
    Receive_JsonPath_DetectedOjbects = 17
    Receive_JsonPath_SubCategory_Top = 18
    Receive_JsonPath_SubCategory_Bottom = 19
    Receive_JsonPath_SubCategory_Outer = 20
    Receive_JsonPath_SubCategory_Overall = 21
    Receive_JsonPath_Pattern_Top = 22
    Receive_JsonPath_Pattern_Bottom = 23
    Receive_JsonPath_Pattern_Outer = 24
    Receive_JsonPath_Pattern_Overall = 25
    Receive_JsonPath_Style = 26
    Receive_Fail = 27

class MessageProductAble(metaclass=ABCMeta):
    @abstractmethod
    def product(self, message):
        #message를 추가해 주는 연산을 해주어야함
        pass

class WorkMessage:
    def setMessage(self, ip, productor :MessageProductAble, type:MessageType, resource:DictUseAbleContainer):
        self.requestor_ip = ip
        self.productor = productor
        self.type = type
        self.resource = resource

    def print(self):
        print("=====================")
        print("ip :\t\t\t",self.requestor_ip)
        print("productor type :\t",type(self.productor))
        print("type :\t\t\t",self.type.name)
        print("resource :\t\t", self.resource.GetDict())
        print("=====================")

class SimpleMessageProduct(MessageProductAble):
    name = "SimpleMessageProduct"
    def __init__(self, q=None):
        self.orderQ = q

    def product(self, message: WorkMessage):
        if(self.orderQ == None):
            raise TypeError
        self.orderQ.put(pickle.dump(message))

    def setQ(self, Q:Queue):
        self.orderQ = Q

    def getName(self):
        return self.name

class EmptyMessageProduct(MessageProductAble):
    name = "EmptyMessageProduct"
    def product(self, message: WorkMessage):
        pass

    def getName(self):
        return self.name

class MessageCounsumeAble(metaclass=ABCMeta):
    @abstractmethod
    def getProductor(self):
        #return MessageProductAble
        pass

    @abstractmethod
    def consume(self):
        #return WorkMessage
        pass

    @abstractmethod
    def emptyQ(self):
        #return ture false(q_empty)
        #하지만 반환값의 신뢰성은 떨어진다.
        pass





class testContainer(DictUseAbleContainer):
    key="test"
    def __init__(self):
        self.container = dict()
        self.container[self.key] = "empty"
    def getDict(self):
        return self.container
    def setDict(self, value_dict):
        self.container[self.key] = value_dict[self.key]
    def getKeyName(self):
        return self.key
    def setValue(self, value):
        self.container[self.key] = value

def testWork(name, productor:MessageProductAble, messageType:MessageType, num=10):
    message = WorkMessage()
    for i in range(num):
        container = testContainer()
        container.setValue(name+"_resource_"+i.__str__())
        print(container.container)
        message.setMessage(name, EmptyMessageProduct(), messageType, container.getDict())
        productor.product(message)

class testMessageConsumer(MessageCounsumeAble):
    def __init__(self):
        self.orderQ = SimpleQueue()

    def getProductor(self):
        q = SimpleMessageProduct(self.orderQ)
        return q

    def consume(self):
        return pickle.load(self.orderQ.get())

    def emptyQ(self):
        #
        return self.orderQ.qsize()


if __name__ == '__main__':
    freeze_support()
    consumer = testMessageConsumer()
    work1 = Process(target=testWork,
                    args=["work1",
                          consumer.getProductor(),
                          MessageType.Request_ImageAnalysis_ImagePath,
                          10])
    """work2 = Process(target=testWork,
                    args=["work2",
                          consumer.getProductor(),
                          MessageType.Receive_ImagePath_RemoveBG,
                          10])"""
    """work3 = Process(target=testWork,
                    args=["work3",
                          consumer.getProductor(),
                          MessageType.Receive_JsonPath_SubCategory_Outer,
                          10])"""
    work1.start()
    #work2.start()
    #work3.start()

    import time
    time.sleep(2)

    while consumer.emptyQ():
        print(consumer.consume())
