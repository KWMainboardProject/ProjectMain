<!---exposed page-->
<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Information Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />

        <!--AOS사용-->
        <link href="https://unpkg.com/aos@2.3.1/dist/aos.css" rel="stylesheet"> 
        <script src="https://unpkg.com/aos@2.3.1/dist/aos.js"></script> 
        <script src="https://code.jquery.com/jquery-1.11.3.min.js"></script> 
    </head>
    <body class="d-flex flex-column">
        <script>
            AOS.init();
        </script>
        <main class="flex-shrink-0">
            <!-- Navigation-->
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark">
                <div class="container px-5">
                    <a class="navbar-brand" href="index.php">MainBoard</a>
                    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation"><span class="navbar-toggler-icon"></span></button>
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <ul class="navbar-nav ms-auto mb-2 mb-lg-0">
                            <li class="nav-item"><a class="nav-link" href="index.php">Home</a></li>
                            <li class="nav-item"><a class="nav-link" href="about.php">About</a></li>
                            <li class="nav-item"><a class="nav-link" href="login.php">Login</a></li>
                            <li class="nav-item dropdown">
                                <a class="nav-link dropdown-toggle" id="navbarDropdownBlog" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Collections</a>
                                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="navbarDropdownBlog">
                                    <li><a class="dropdown-item" href="Outer-list.php">Outer</a></li>
                                    <li><a class="dropdown-item" href="Top-list.php">Top</a></li>
                                    <li><a class="dropdown-item" href="Bottom-list.php">Bottom</a></li>
                                    <li><a class="dropdown-item" href="Overall-list.php">Overall</a></li>
                                </ul>
                            </li>
                            <li class="nav-item"><a class="nav-link" href="product_create.php">Product Register</a></li>
                            <li name="nav_myproducts" class="nav-item"><a class="nav-link" href="myProduct-list.php" style="display: none;">My Products</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Header-->
            <header class="py-5" style="background-color: orange;">
                <div class="container px-5" data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                    <div class="row justify-content-center">
                        <div class="col-lg-8 col-xxl-6">
                            <div class="text-center my-5">
                                <h1 class="fw-bolder mb-3">Our mission is to find clothes <br> that are similar to the picture.</h1>
                                <p class="lead fw-normal text-muted mb-4">과거 온라인 쇼핑몰에 비하여, AI 알고리즘 도입은 판매량의 상승과<br>직결됨을 확인할 수 있습니다. 보다 정밀한 AI 알고리즘 기술은<br>온라인 의류 플랫폼에서 상당한 경쟁력을 가지게 할 수 있고,<br>보다 차별화된 기술은 판매량 상승에 큰 기여를 할 수 있는 바,<br>필요성이 점점 부각되고 있습니다.</p>
                                <a class="btn btn-primary btn-lg" href="#scroll-target">Read our Technology</a>
                            </div>
                        </div>
                    </div>
                </div>
            </header>
            <!-- About CNN section-->
            <section class="py-5 bg-light" id="scroll-target">
                <div class="container px-5 my-5" data-aos="zoom-in">
                    <div class="row gx-5 align-items-center">
                        <div class="col-lg-6">
                            <img class="img-fluid rounded mx-auto d-block mb-5 mb-lg-0" src="image/clothes_cnn.png" alt="..."/>
                        </div>
                        <div class="col-lg-6">
                            <h2 class="fw-bolder">CNN</h2>
                            <p class="lead fw-normal text-muted mb-0" style="font-size: 1em;">
                                CNN은 사람의 시신경을 모방하여 설계된 인공지는 모델로,
                                이미지를 분석하기 위한 패턴을 찾는데 유용한 알고리즘입니다.
                                CNN은 이미지를 그대로 받음으로써 공간/지역적 정보를 유지한채
                                특성을 추출한다는 점이 특징적입니다. CNN은 이러한 이미지의 특징들을 종합하여
                                이미지를 분류하며, 이와같은 특성으로 인해 기존의 딥러닝보다 이미지 분류에 높은 성능을 보이고 있습니다.
                            </p>
                        </div>
                    </div>
                </div>
            </section>
            <!-- About Yolo V5 section-->
            <section class="py-5">
                <div class="container px-5 my-5" data-aos="zoom-in">
                    <div class="row gx-5 align-items-center">
                        <div class="col-lg-6 order-first order-lg-last">    
                            <img class="img-fluid rounded mx-auto d-block mb-5 mb-lg-0" src="image/yolo.png" alt="..."/>
                        </div>
                        <div class="col-lg-6">
                            <h2 class="fw-bolder">Yolo v5 (You Only Look Once)</h2>
                            <p class="lead fw-normal text-muted mb-0" style="font-size: 1em;">
                                Yolo는  Object Detection 모델로 Single-Stage Methods를 사용한 모델입니다.
                                정확도는 다소 떨어지나 연산 속도가 매우 빠른 장점을 가지고 있습니다. Object Detection 모델
                                CNN을 사용해 하나의 분류을 넘어 영상 안에 존재하는 객체 각 좌표와 분류를 찾아내는
                                모델 Single-Stage Methods 영상 내 존재하는 객체들을 탐지하는 방법이 존재하는데,
                                그 방법은 영역 제안 방식(Region Proposal Methods)을 수정한 방식으로 객체가 존재할 만한
                                영역을 찾아내고, 영역 내에 객체의 분류를 찾는 방식입니다.
                            </p>
                        </div>
                    </div>
                </div>
            </section>
            <!-- About ResNet section-->
            <section class="py-5">
                <div class="container px-5 my-5" data-aos="zoom-in">
                    <div class="row gx-5 align-items-center">
                        <div class="col-lg-6">
                            <img class="img-fluid rounded mx-auto d-block mb-5 mb-lg-0" src="image/resnet.png" alt="..."/>
                        </div>
                        <div class="col-lg-6">
                            <h2 class="fw-bolder">ResNet</h2>
                            <p class="lead fw-normal text-muted mb-0" style="font-size: 1em;">
                                ResNet은 2015년도 ILSVRC에서 우승한 모델로, 총 152개의 층을 가진 깊은 네트워크이며,
                                상위 계층에서 의미 있는 특징 추출이 가능하나 깊은 층을 사용 시 기울기 소멸이 발생한다는 점을
                                잔차 모듈을 통해 해결하였습니다. 잔차 모듈은 기대하는 출력과 유사한 입력이 들어오면 영벡터에
                                가까운 값을 학습하기에, 입력의 작은 변화에 민감합니다. 또한 다양한 경로를 통해 복합적인 특징을
                                추출합니다. 이는 필요한 출력이 얻어지면 컨볼루션 층을 건너뛸 수 있고, 다양한 조합의 특징 추출이
                                가능합니다.
                            </p>
                        </div>
                    </div>
                </div>
            </section>
            <!-- About Whole Process section-->
            <section class="py-5">
                <div class="container px-5 my-5" data-aos="zoom-in">
                    <div class="row gx-5 align-items-center">
                        <div class="col-lg-6 order-first order-lg-last">
                            <img class="img-fluid rounded mx-auto d-block mb-5 mb-lg-0" src="image/process.png" alt="..."/>
                        </div>
                        <div class="col-lg-6">
                            <h2 class="fw-bolder">Whole Process</h2>
                            <p class="lead fw-normal text-muted mb-0" style="font-size: 1em;">Lorem ipsum dolor sit amet consectetur adipisicing elit. Iusto est, ut esse a labore aliquam beatae expedita. Blanditiis impedit numquam libero molestiae et fugit cupiditate, quibusdam expedita, maiores eaque quisquam.</p>
                        </div>
                    </div>
                </div>
            </section>
            <!-- Team members section-->
            <section class="py-5 bg-light">
                <div class="container px-5 my-5">
                    <div class="text-center">
                        <h2 class="fw-bolder">Our team</h2>
                        <p class="lead fw-normal text-muted mb-5">Dedicated to quality and your comfort</p>
                    </div>
                    <div class="row gx-5 row-cols-1 row-cols-sm-2 row-cols-xl-4 justify-content-center">
                        <div class="col mb-5 mb-5 mb-xl-0">
                            <div class="text-center">
                                <img class="img-fluid rounded-circle mb-4 px-4" src="https://dummyimage.com/150x150/ced4da/6c757d" alt="..." />
                                <h5 class="fw-bolder">손우진</h5>
                                <div class="fst-italic text-muted">Team Manager <br> Frond-end / Database, Front-Back-DB communication</div>
                            </div>
                        </div>
                        <div class="col mb-5 mb-5 mb-xl-0">
                            <div class="text-center">
                                <img class="img-fluid rounded-circle mb-4 px-4" src="https://dummyimage.com/150x150/ced4da/6c757d" alt="..." />
                                <h5 class="fw-bolder">이탁균</h5>
                                <div class="fst-italic text-muted">Team member <br> Back-end / YoloV5, Color extraction, Remove background</div>
                            </div>
                        </div>
                        <div class="col mb-5 mb-5 mb-sm-0">
                            <div class="text-center">
                                <img class="img-fluid rounded-circle mb-4 px-4" src="https://dummyimage.com/150x150/ced4da/6c757d" alt="..." />
                                <h5 class="fw-bolder">정유섭</h5>
                                <div class="fst-italic text-muted">Team member <br> Frond-end / CNN Learning, Color extracion, Remove background</div>
                            </div>
                        </div>
                        <div class="col mb-5">
                            <div class="text-center">
                                <img class="img-fluid rounded-circle mb-4 px-4" src="https://dummyimage.com/150x150/ced4da/6c757d" alt="..." />
                                <h5 class="fw-bolder">홍명준</h5>
                                <div class="fst-italic text-muted">Team member <br> Back-end / CNN Learning, Color extraction</div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </main>
        <!-- Footer-->
        <footer class="bg-dark py-4 mt-auto">
            <div class="container px-5">
                <div class="row align-items-center justify-content-between flex-column flex-sm-row">
                    <div class="col-auto"><div class="small m-0 text-white">Copyright &copy; MainBoard 2022</div></div>
                    <div class="col-auto">
                        <a class="link-light small" href="#!">Privacy</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Terms</a>
                        <span class="text-white mx-1">&middot;</span>
                        <a class="link-light small" href="#!">Contact</a>
                    </div>
                </div>
            </div>
        </footer>
        <!-- Bootstrap core JS-->
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
        <!-- Core theme JS-->
        <script src="js/scripts.js"></script>
    </body>
</html>
