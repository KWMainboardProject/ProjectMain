import torch
from torch.nn import functional
from torchvision import transforms
from PIL import Image
import os
import matplotlib.pyplot as plt
import numpy as np

class Top_classification:
    top_class = ['Hoodie', 'Long sleeve shirts', 'Long sleeve tee', 'Pullover', 'Short sleeve shirts', 'Short sleeve tee', 'Sleeveless', 'Turtleneck']
    dev = "cuda:0"

    def __init__(self, dev_value, img_path, model_path):
        self.set_dev = dev_value
        self.img_path = img_path
        self.model_path = model_path

    #디바이스 설정 함수
    def set_device(self):
        if self.set_dev==0:
            device = torch.device("cuda:0")
            self.dev=device
        else:
            device = torch.device("cpu")
            self.dev=device

    #이미지 로드 함수
    def img_load(self):
        path = self.img_path
        origin_img = Image.open(path).convert('RGB')
        np_img = np.array(origin_img)
        img = Image.fromarray(np_img)
        self.img = img

    #이미지 전처리 함수
    def img_transform(self):
        pre_transforms = transforms.Compose([
            transforms.Resize((224, 224)),
            transforms.ToTensor(),
            transforms.Normalize([0.485, 0.456, 0.406], [0.229, 0.224, 0.225])
        ])
        pre_img = pre_transforms(self.img)
        self.pre_img = pre_img

    #신경망 로드 함수
    def NN_load(self):
        model = torch.load(self.model_path)
        self.model = model

    #신경망 돌려서 결과 리턴하는 함수
    def classification(self):
        model = self.model
        img = self.pre_img
        model.eval()
        with torch.no_grad():
            input = img.to(self.dev)
            output = model(input[None, ...])
            probs = torch.nn.functional.softmax(output, dim=1)
            _, preds = torch.max(probs, 1)
            conf = _
            result = self.top_class[preds[0]]
        return conf, result

"""
tc = Top_classification()
path = os.path.abspath("./후드티.jpg")
print("path: ", path)

tc.set_device(0)

print(">>>Stage1: set device")
print()

img = tc.img_load(path)
print(type(img))
plt.imshow(img)
plt.show()

print(">>>Stage2: image load")
print()

pre_img = tc.img_transform(img)

print(">>>Stage3: image preprocessing")
print()

model = tc.NN_load()

print(">>>Stage4: Neural Network load")
print()

conf, result = tc.classification(model, pre_img)
print("Predict Result: ",result)
print(">>>Stage5: get Image's result class")
"""