<?php
/**
 * ===========================
 * 공통 헤더 레이아웃 (Common Header Layout)
 * ===========================
 *
 * 모든 페이지에서 공통으로 사용되는 상단 부분
 * Top section used commonly across all pages
 *
 * 포함 내용 (Includes):
 * - HTML doctype 및 head 태그
 * - 메타 태그
 * - CSS 링크
 * - 페이지 타이틀
 */

// config.php가 로드되었는지 확인
// Check if config.php is loaded
if (!defined('APP_NAME')) {
    die('Configuration not loaded. Please include config.php first.');
}

// 페이지별 타이틀 설정 (각 페이지에서 $pageTitle 변수로 전달)
// Set page-specific title (passed via $pageTitle variable from each page)
$title = isset($pageTitle) ? getPageTitle($pageTitle) : DEFAULT_PAGE_TITLE;
?>
<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title><?= htmlspecialchars($title) ?></title>

    <!--
        스타일시트 (Stylesheets)
        - styles.css: 애플리케이션 전체 스타일
    -->
    <link rel="stylesheet" href="styles.css">

    <?php
    /**
     * 페이지별 추가 CSS
     * Page-specific additional CSS
     *
     * 각 페이지에서 $additionalCss 배열로 전달
     * Passed from each page via $additionalCss array
     *
     * 예 (Example):
     * $additionalCss = ['custom.css', 'datepicker.css'];
     */
    if (isset($additionalCss) && is_array($additionalCss)) {
        foreach ($additionalCss as $css) {
            echo '<link rel="stylesheet" href="' . htmlspecialchars($css) . '">' . "\n";
        }
    }
    ?>

    <?php
    /**
     * 인라인 스타일
     * Inline styles
     *
     * 각 페이지에서 $inlineStyles로 전달
     * Passed from each page via $inlineStyles
     */
    if (isset($inlineStyles)) {
        echo '<style>' . $inlineStyles . '</style>';
    }
    ?>
</head>
<body>
    <!--
        메인 컨테이너 시작
        Main container start

        각 페이지의 컨텐츠가 여기 안에 들어감
        Each page's content goes inside here
    -->
