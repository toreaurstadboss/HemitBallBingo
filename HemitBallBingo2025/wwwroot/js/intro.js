/* Intro page — logo animation + prize counter
   Requires: GSAP loaded before this file, window.introConfig.prizeAmount set inline. */

/* Prize Counter --------------------------------------------------------- */
const MINIMUM_ADDITIONAL_ITERATION_COUNT = 2;

const config = {
    additionalIterationCount: Math.max(MINIMUM_ADDITIONAL_ITERATION_COUNT, 3),
    transitionDuration: 3000,
    prize: (window.introConfig && window.introConfig.prizeAmount) || 0,
    digits: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
};

const NOK = new Intl.NumberFormat("nb-NO", {
    style: "currency",
    currency: "NOK",
    maximumFractionDigits: 0
});

const getPrizeText = () => document.getElementById("prize-text");
const getTracks = () => document.querySelectorAll(".digit > .digit-track");
const getFormattedPrize = () => NOK.format(config.prize);
const getPrizeDigitByIndex = index => parseInt(config.prize.toString()[index]);
const determineIterations = index => index + config.additionalIterationCount;

const createElement = (type, className, text) => {
    const el = document.createElement(type);
    el.className = className;
    if (text !== undefined) el.innerText = text;
    return el;
};

const createCharacter = character => createElement("span", "character", character);

const createDigit = (digit, trackIndex) => {
    const digitEl = createElement("span", "digit");
    const trackEl = createElement("span", "digit-track");

    let digits = [];
    const iterations = determineIterations(trackIndex);
    for (let i = 0; i < iterations; i++) digits = [...digits, ...config.digits];

    trackEl.innerText = digits.join(" ");
    trackEl.style.transitionDuration = `${config.transitionDuration}ms`;
    digitEl.appendChild(trackEl);
    return digitEl;
};

const setup = () => {
    let index = 0;
    const prizeText = getPrizeText();
    for (const character of getFormattedPrize()) {
        prizeText.appendChild(
            isNaN(character) ? createCharacter(character) : createDigit(character, index++)
        );
    }
};

const animate = () => {
    getTracks().forEach((track, index) => {
        const digit = getPrizeDigitByIndex(index);
        const iterations = determineIterations(index);
        const activeDigit = (iterations - 1) * 10 + digit;
        track.style.translate = `0rem ${activeDigit * -10}rem`;
    });
};

/* Theme ----------------------------------------------------------------- */
const updateTheme = theme => {
    document.documentElement.style.setProperty("--theme-rgb", `var(--${theme})`);
    document.querySelectorAll(".theme-button").forEach(btn => {
        btn.dataset.selected = theme === btn.dataset.theme;
    });
};

document.querySelectorAll(".theme-button").forEach(btn => {
    btn.addEventListener("click", () => updateTheme(btn.dataset.theme));
});

/* GSAP Logo Animation --------------------------------------------------- */
const red = "#fe3477";
const D = 0.5;

gsap.set("#Top_circle #tc_right", { strokeDashoffset: -501, opacity: 0 });
gsap.set("#left_circle2", { strokeDashoffset: 467, opacity: 0 });
gsap.set("#left_circle1", { strokeDashoffset: -328 });
gsap.set("#right_line1", { attr: { y2: 281.63 } });
gsap.set("#right_line2", { attr: { y1: 650.29 } });
gsap.set("#right_line3", { attr: { y1: 578.54 } });
gsap.set("#right_line4", { attr: { x2: 1061.44 } });
gsap.set("#seven", { strokeDashoffset: 245 });
gsap.set("#bottom_circle", { r: 0 });
gsap.set("#top_circle_2", { r: 0 });
gsap.set("#top_line1", { attr: { y1: 448.39 } });
gsap.set("#top_line2", { attr: { x1: 1052.14 } });
gsap.set("#left_line1", { attr: { y2: 281.63 } });
gsap.set("#left_line_2", { attr: { x2: 694.86 } });
gsap.set("#left_line_3", { attr: { y2: 495.4 } });
gsap.set("#left_line_4", { attr: { x1: 674.73 } });
gsap.set("svg", { opacity: 1 });

const tl = gsap.timeline({ delay: 0.5 });

/* --- Center triangle --- */
tl.to("#Center_triangle #ct_l", { x: 280, duration: 3.4 * D, ease: "power2.inOut" }, 0);
tl.to("#Center_triangle #ct_r", { x: 140, y: 237, duration: 3.4 * D, ease: "power2.inOut" }, 0);
tl.to("#Center_triangle #ct_top, #ct_mask_top", { x: -340, duration: 3.5 * D, ease: "none" }, 3.4 * D);
tl.to("#ct_b, #ct_mask_bottom", { x: -60, duration: 0.67 * D, ease: "none" }, (3.4 * D + (3.5 * D - 0.67 * D)));

/* --- Top circle --- */
tl.to("#Top_circle #tc_top", { strokeDashoffset: -753, duration: 2.16 * D, ease: "power2.inOut" }, 1.45 * D);
tl.set("#Top_circle #tc_top", { opacity: 0 }, (1.45 * D + 2.16 * D));
tl.set("#Top_circle #tc_right", { opacity: 1 }, (1.45 * D + 2.16 * D));
tl.to("#Top_circle #tc_right", { strokeDashoffset: 27, duration: 3 * D, ease: "power2.inOut" }, (1.45 * D + 2.16 * D));

/* --- Bottom Rect --- */
tl.to("#Bottom_Rect #br_top", { attr: { x1: 1368 }, duration: 2 * D, ease: "power3.inOut" }, 1 * D);
tl.to("#Bottom_Rect #br_top", { attr: { x2: 1368 }, duration: 2 * D, ease: "power1.inOut" }, 1 * D);
tl.to("#Bottom_Rect #br_top", { attr: { y1: 471, y2: 559 }, duration: 0.8 * D, ease: "power1.inOut" }, (1.45 * D + 2 * D));

tl.to("#Bottom_Rect #br_right", { attr: { y1: 471 }, duration: 1.8 * D, ease: "power3.inOut" }, 1.45 * D);
tl.to("#Bottom_Rect #br_right", { attr: { y2: 471 }, duration: 1.8 * D, ease: "power1.inOut" }, 1.45 * D);
tl.to("#Bottom_Rect #br_right", { attr: { x1: 1280, x2: 1368 }, duration: 1 * D, ease: "power1.inOut" }, (1.45 * D + 1.8 * D));

tl.to("#Bottom_Rect #br_bottom", { attr: { x1: 1280 }, duration: 3 * D, ease: "power3.inOut" }, 0);
tl.to("#Bottom_Rect #br_bottom", { attr: { x2: 1280 }, duration: 3 * D, ease: "power1.inOut" }, 0);
tl.to("#Bottom_Rect #br_bottom", { attr: { y1: 471 }, duration: 1 * D, ease: "power1.inOut" }, 3.5 * D);
tl.to("#Bottom_Rect #br_bottom", { attr: { y2: 559 }, duration: 2 * D, ease: "power2.inOut" }, 3.5 * D);

tl.to("#Bottom_Rect #br_left", { attr: { y1: 559, y2: 559 }, duration: 1 * D, ease: "power2.inOut" }, 2 * D);
tl.to("#Bottom_Rect #br_left", { attr: { x1: 1280 }, duration: 4 * D, ease: "power2.inOut" }, 3.2 * D);
tl.to("#Bottom_Rect #br_left", { attr: { x2: 1368 }, duration: 3 * D, ease: "power1.inOut" }, 3.2 * D);

/* --- Random lines --- */
tl.to("#right_line1", { attr: { y2: 440 }, duration: 1 * D, ease: "power2.inOut" }, 4.8 * D);
tl.to("#right_line2", { attr: { y1: 462 }, duration: 1 * D, ease: "power1.inOut" }, 4.4 * D);
tl.to("#right_line3", { attr: { y1: 462 }, duration: 1.5 * D, ease: "power1.inOut" }, 2.8 * D);
tl.to("#right_line4", { attr: { x2: 1139.51 }, duration: 1 * D, ease: "power1.inOut" }, 5.6 * D);
tl.to("#seven", { strokeDashoffset: 0, duration: 1.6 * D, ease: "power1.inOut" }, 4.8 * D);
tl.to("#bottom_circle", { r: 32, duration: 2 * D, ease: "power1.out" }, 4 * D);
tl.to("#top_line1", { attr: { y1: 281 }, duration: 2 * D, ease: "power1.inOut" }, 4.7 * D);
tl.to("#top_line2", { attr: { x1: 974.94 }, duration: 1 * D, ease: "power1.inOut" }, 5.5 * D);
tl.to("#top_circle_2", { r: 32, duration: 2 * D, ease: "power1.out" }, 4.4 * D);
tl.to("#left_line1", { attr: { y2: 448.39 }, duration: 1.4 * D, ease: "power2.inOut" }, 5 * D);
tl.to("#left_line_2", { attr: { x2: 821.69 }, duration: 1.4 * D, ease: "power2.inOut" }, 3.4 * D);
tl.to("#left_line_3", { attr: { y2: 414.25 }, duration: 1 * D, ease: "power2.inOut" }, 5 * D);
tl.to("#left_line_4", { attr: { x1: 481.35 }, duration: 3.5 * D, ease: "power2.inOut" }, 0);

/* --- Left circle --- */
tl.to("#left_circle1", { strokeDashoffset: 400, duration: 2.8 * D, ease: "power2.inOut" }, 1 * D);
tl.set("#left_circle1", { opacity: 0 }, (1 * D + 2.8 * D));
tl.set("#left_circle2", { opacity: 1 }, (1 * D + 2.8 * D));
tl.to("#left_circle2", { strokeDashoffset: 1343, duration: 3.5 * D, ease: "power2.inOut" }, (1 * D + 3 * D));

/* --- Colors turn red --- */
tl.to("#br_top, #br_bottom, #br_right, #br_left, #left_circle2", { stroke: red, duration: 3 * D, ease: "power2.inOut" }, 4.5 * D);
tl.to("#ct_b, #ct_r, #ct_l", { fill: red, duration: 3 * D, ease: "power2.inOut" }, 4.5 * D);

/* --- Shadows --- */
tl.set("#Center_triangle", { attr: { filter: "url(#dropshadow_top)" } }, 3 * D);
tl.to("#dropshadow_shade feFuncA", { attr: { slope: 0.5 }, duration: 0.4 * D }, 4 * D);
tl.to("#dropshadow_shade feFuncA", { attr: { slope: 0 }, duration: 1 * D }, 5.5 * D);

/* --- After logo: fade SVG out, reveal "Hemit Ball Bingo" title --- */
tl.to("svg", { opacity: 0, scale: 0.8, duration: 1.5, ease: "power2.inOut" }, "+=1");

tl.fromTo("#introText",
    { opacity: 0, scale: 1.3, y: 30 },
    { opacity: 1, scale: 1, y: 0, duration: 2, ease: "power3.out" },
    "-=0.5"
);

tl.to("#introText", {
    textShadow: "0 0 60px rgba(254, 52, 119, 0.9), 0 0 120px rgba(254, 52, 119, 0.5)",
    duration: 1.2,
    ease: "power1.inOut",
    yoyo: true,
    repeat: 1
});

/* Init ------------------------------------------------------------------ */
setup();
setTimeout(animate);
updateTheme("green");
