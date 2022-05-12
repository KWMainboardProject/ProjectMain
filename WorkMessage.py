from abc import *
from enum import Enum
from class_file import DictUseAbleContainer
from multiprocessing import Queue

class MessageType(Enum):
    Empty_Message = 1

class MessageProductAble(metaclass=ABCMeta):
    @abstractmethod
    def Product(self, message):
        pass

class WorkMessage:
    def setMessage(self, ip, productor :MessageProductAble, type:MessageType, resource:DictUseAbleContainer):
        self.requestor_ip = ip
        self.productor = productor
        self.type = type
        self.resource = resource

class SimpleMessageProduct(MessageProductAble):
    def __init__(self):
        self.orderQ = Queue();

    def Product(self, message: WorkMessage):
        self.orderQ.put(message)

