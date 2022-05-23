<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
        <meta name="description" content="" />
        <meta name="author" content="" />
        <title>MainBoard Image Search Result Page</title>
        <!-- Favicon-->
        <link rel="icon" type="image/x-icon" href="assets/favicon.ico" />
        <!-- Bootstrap icons-->
        <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.5.0/font/bootstrap-icons.css" rel="stylesheet" />
        <!-- Core theme CSS (includes Bootstrap)-->
        <link href="css/styles.css" rel="stylesheet" />

        <script src="example.js"></script>
    </head>
    <body class="d-flex flex-column h-100">
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
                            <!--내 등록 상품 페이지로 연결. display값 default는 none이고, 로그인 시 block로 변경-->
                            <li name="nav_myproducts" class="nav-item"><a class="nav-link" href="myProduct-list.php" style="display: none;">My Products</a></li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Page Content-->
            <section class="py-5">
                <div class="container">
                    <h2 class="mb-5" style="font-weight: bolder;">Similar Image Result</h2>
                    <hr class="mb-5">
                    <!--각 추천 section의 visibility default value = hidden-->
                    <!--Outer 추천 (Outer가 존재한다면 visibility hidden->visible)-->
                    <div class="mt-5 row g-3" style="visibility: visible;"  data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                        <h4 class="mb-3">Similar Outer Recommend</h4>
                        <!--카드 하나 컴포넌트 => DB에서 for문으로 5개 가져와야함-->
                        <div class="col xl-3 col-lg-4 col-md-6">
                            <div class="card" style="width: 300px;">
                                <!--이미지 버튼화-->
                                <a href="product-info.php">
                                    <!--상품 썸네일 DB 연동-->
                                    <img src="" class="card-img-top" style="width: 300px; height: 400px; object-fit: contain;">
                                </a>
                                <div class="card-body">
                                    <!--상품명, 가격 DB연동-->
                                    <h5 class="card-title">Product Name</h5>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <p>Product Price</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Top 추천 (Top이 존재한다면 visibility hidden->visible)-->
                    <div class="mt-5 row g-3" style="visibility: visible;"  data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                        <h4 class="mb-3">Similar Top Recommend</h4>
                        <!--카드 하나 컴포넌트 => DB에서 for문으로 5개 가져와야함-->
                        <div class="col xl-3 col-lg-4 col-md-6">
                            <div class="card" style="width: 300px;">
                                <!--이미지 버튼화-->
                                <a href="product-info.php">
                                    <!--상품 썸네일 DB 연동-->
                                    <img src="" class="card-img-top" style="width: 300px; height: 400px; object-fit: contain;">
                                </a>
                                <div class="card-body">
                                    <!--상품명, 가격 DB연동-->
                                    <h5 class="card-title">Product Name</h5>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <p>Product Price</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Bottom 추천 (Bottom이 존재한다면 visibility hidden->visible)-->
                    <div class="mt-5 row g-3" style="visibility: visible;"  data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                        <h4 class="mb-3">Similar Bottom Recommend</h4>
                        <!--카드 하나 컴포넌트 => DB에서 for문으로 5개 가져와야함-->
                        <div class="col xl-3 col-lg-4 col-md-6">
                            <div class="card" style="width: 300px;">
                                <!--이미지 버튼화-->
                                <a href="product-info.php">
                                    <!--상품 썸네일 DB 연동-->
                                    <img src="" class="card-img-top" style="width: 300px; height: 400px; object-fit: contain;">
                                </a>
                                <div class="card-body">
                                    <!--상품명, 가격 DB연동-->
                                    <h5 class="card-title">Product Name</h5>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <p>Product Price</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Overall 추천 (Overall이 존재한다면 visibility hidden->visible)-->
                    <div class="mt-5 row g-3" style="visibility: visible;"  data-aos="fade-up" aos-offset="300" aos-easing="ease-in-sine" aos-duration="500">
                        <h4 class="mb-3">Similar Overall Recommend</h4>
                        <!--카드 하나 컴포넌트 => DB에서 for문으로 가져와야함-->
                        <div class="col xl-3 col-lg-4 col-md-6">
                            <div class="card" style="width: 300px;">
                                <!--이미지 버튼화-->
                                <a href="product-info.php">
                                    <!--상품 썸네일 DB 연동-->
                                    <img src="" class="card-img-top" style="width: 300px; height: 400px; object-fit: contain;">
                                </a>
                                <div class="card-body">
                                    <!--상품명, 가격 DB연동-->
                                    <h5 class="card-title">Product Name</h5>
                                    <div class="d-flex justify-content-between align-items-center">
                                        <p>Product Price</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mt-5 row g-3">
                        <a href="index.php" class="col-12 d-grid p-1">
                                <button type="button" class="btn btn-lg btn-outline-primary">Home</button>
                        </a>
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