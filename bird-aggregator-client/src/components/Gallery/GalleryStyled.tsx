import styled from 'styled-components'

export const HeaderStyled = styled.header`
    top: var(--yarl__slide_title_container_top, 0);
    background: var(--yarl__slide_captions_container_background, rgba(0, 0, 0, 0.5));
    left: var(--yarl__slide_captions_container_left, 0);
    padding: var(--yarl__slide_captions_container_padding, 16px);
    position: absolute;
    right: var(--yarl__slide_captions_container_right, 0);
    -webkit-transform: translateZ(0);
    transform: translateZ(0);
    color: white;
    font-size: 1.3em;
`

export const BirdLinkStyled = styled.span`
    color: white;
    font-size: 1.3em;
`

export const TimestampStyled = styled.span`
    float: right;
    font-size: 0.6em;
    margin-right: 50px;
`

export const GalleryStyled = styled.div`
    margin: 5px;
    margin-top: 20px;
`

export const FooterStyled = styled.footer`
    color: white;
    font-size: 1em;
    position: absolute;
    bottom: 0;
    background-color: #5050508c;
    width: 100%;
    padding-bottom: 5px;
    padding-left: 20px;
    padding-top: 5px;
`
