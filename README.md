# Mainboard
**🧥Clothing search system using AI**
KwangWoon University SW Partnership Project
(21.08 ~ 22.05)

Visit to http://uijin97.cafe24.com for experience our project!


이미지 분석 서버 setting
1. 신경망 다운로드
다운로드 링크
- yolov5 - fashion detector
https://drive.google.com/file/d/1u3uS9ZpjCUmRnygRs2looIxrdRvXcSYf/view?usp=sharing

- u2net - segmentation (https://github.com/xuebinqin/U-2-Net)
https://drive.google.com/file/d/1vSmKAFtCiGOudu5_7V2HnA-96rTG3v1Z/view?usp=sharing

- resnet34 - subcategory_top
https://drive.google.com/file/d/182MLAqJ0pNHbvRA_WqeVTQyC8-kflieQ/view?usp=sharing

- resnet34 - subcategory_bottom
https://drive.google.com/file/d/164dWo_t40hsmdG3cHm3HdqWAgmNehU2o/view?usp=sharing

- resnet34 - subcategory_overall
https://drive.google.com/file/d/1VUGialL52mITdG6NjBErSBytcnpkNr4u/view?usp=sharing

- resnet34 - subcategory_outer
https://drive.google.com/file/d/1bHUZ2BG8qluh1YqaUDmoSqdViks8mQEi/view?usp=sharing

- resnet34 - pattern
https://drive.google.com/file/d/1MSL4YsaIKmEE7tv7S8qqglm_pgnOyKFM/view?usp=sharing

2. 파일 setting 폴더
ROOT Directory = C:\Users\Public\ProjectMain
weight 폴더 안에 다운받은 신경망 들을 넣는다.
KEY 폴더 안에 key.json 파일을 넣고 내용은 아래 예시와 같이 넣는다.
{"KEY": {"IP": "000.111.222.34:56","ID": "admin123123","PW": "1qaz2wsx3edc}}

참고) root의 위치를 바꾸고 싶다면 WorkResourceClass의 ShareWorkPath의 코드를 수정하면 된다.

3. Nuget설치
- Microsoft.ML.OnnxRuntime
- Microsoft.ML.OnnxRuntime.Gpu(X)
- Microsoft.ML.OnnxRuntime.Managed
- Newtonsoft.Json
- OpenCvSharp3-AnyCPU

