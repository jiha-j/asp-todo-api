    <!--
        ===========================
        공통 푸터 레이아웃 (Common Footer Layout)
        ===========================

        모든 페이지에서 공통으로 사용되는 하단 부분
        Bottom section used commonly across all pages

        포함 내용 (Includes):
        - 푸터 정보
        - JavaScript 파일 로드
        - body 및 html 태그 닫기
    -->

    <?php
    /**
     * 페이지별 JavaScript 파일
     * Page-specific JavaScript files
     *
     * 각 페이지에서 $jsFiles 배열로 전달
     * Passed from each page via $jsFiles array
     *
     * 예 (Example):
     * $jsFiles = ['app.js', 'chart.js'];
     */
    if (isset($jsFiles) && is_array($jsFiles)) {
        foreach ($jsFiles as $js) {
            echo '<script src="' . htmlspecialchars($js) . '"></script>' . "\n    ";
        }
    }
    ?>

    <?php
    /**
     * 인라인 JavaScript
     * Inline JavaScript
     *
     * 각 페이지에서 $inlineJs로 전달
     * Passed from each page via $inlineJs
     */
    if (isset($inlineJs)) {
        echo '<script>' . $inlineJs . '</script>';
    }
    ?>

    <?php
    /**
     * API URL을 JavaScript에 전달
     * Pass API URL to JavaScript
     *
     * PHP 상수를 JavaScript에서 사용할 수 있도록 설정
     * Make PHP constants available in JavaScript
     */
    ?>
    <script>
        // PHP에서 JavaScript로 설정 전달
        // Pass configuration from PHP to JavaScript
        const API_BASE_URL = '<?= getApiUrl() ?>';
        const DEBUG_MODE = <?= DEBUG_MODE ? 'true' : 'false' ?>;

        <?php if (DEBUG_MODE): ?>
        // 디버그 정보 출력 (Debug information output)
        console.log('📱 애플리케이션 설정 (Application Config):');
        console.log('- API URL:', API_BASE_URL);
        console.log('- Debug Mode:', DEBUG_MODE);
        console.log('- Current Page: <?= getCurrentPage() ?>');
        <?php endif; ?>
    </script>

</body>
</html>
