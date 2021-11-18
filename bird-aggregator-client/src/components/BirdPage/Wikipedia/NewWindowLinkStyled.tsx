import styled from 'styled-components'

const sameStyles = `
    color: #106ba3;
    text-decoration: none;
    border-bottom: 1px #106ba3 dotted;
`

export const NewWindowLinkStyled = styled.a`
    ${sameStyles}
    &:visited {
        ${sameStyles}
    }

    &:active {
        ${sameStyles}
    }

    &:hover {
        color: #106ba3;
        text-decoration: underline;
        border: none;
      }
    }
`
